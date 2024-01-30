using Microsoft.CodeAnalysis.Operations;
using Prism.SourceGenerators.Extensions;
using Prism.SourceGenerators.Helpers;

namespace Prism.SourceGenerators.Diagnostics.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class FieldUsingAttributeForBindablePropertyAnalyzer : DiagnosticAnalyzer
{
    internal const string FieldNameKey = "FieldName";

    internal const string PropertyNameKey = "PropertyName";

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.BindablePropertyNameCollisionError);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(static context =>
        {
            if (context.Compilation.GetTypeByMetadataName(CodeHelpers.__BindablePropertyFullAttribute__) is not INamedTypeSymbol bindablePropertySymbol)
                return;

            context.RegisterOperationAction(context =>
            {
                if (context.Operation is not IFieldReferenceOperation
                    {
                        Field: IFieldSymbol { IsStatic: false, IsConst: false, IsImplicitlyDeclared: false, ContainingType: INamedTypeSymbol } fieldSymbol,
                        Instance.Type: ITypeSymbol typeSymbol
                    })
                    return;

                if (context.ContainingSymbol is IMethodSymbol { MethodKind: MethodKind.Constructor, ContainingType: INamedTypeSymbol instanceType } &&
                   SymbolEqualityComparer.Default.Equals(instanceType, typeSymbol))
                    return;


                foreach (AttributeData attribute in fieldSymbol.GetAttributes())
                {
                    if (attribute.AttributeClass is { Name: CodeHelpers.__BindablePropertyAttributeEmbeddedResourceName__ } attributeClass &&
                        SymbolEqualityComparer.Default.Equals(attributeClass, bindablePropertySymbol))
                    {
                        var propertyName = fieldSymbol.CreateGeneratedPropertyName();
                        if (fieldSymbol.Name == propertyName)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                    DiagnosticDescriptors.BindablePropertyNameCollisionError,
                                    context.Operation.Syntax.GetLocation(),
                                    ImmutableDictionary.Create<string, string?>()
                                        .Add(FieldNameKey, fieldSymbol.Name)
                                        .Add(PropertyNameKey, propertyName),
                                    fieldSymbol));

                            return;
                        }
                    }
                }

            }, OperationKind.FieldReference);
        });
    }
}
