using Prism.SourceGenerators.Generators;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using static Prism.SourceGenerators.Helpers.CodeHelpers;
using static SourceGeneratorToolkit.Helpers.CommonHelpers;

namespace Prism.SourceGenerators.Diagnostics.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ClassUsingAttributeInsteadOfInheritanceAnalyzer : DiagnosticAnalyzer
{
    internal const string TypeNameKey = "TypeName";

    internal const string AttributeTypeNameKey = "AttributeTypeName";

    private static readonly ImmutableDictionary<string, string> GeneratorAttributeNamesToFullyQualifiedNamesMap = ImmutableDictionary.CreateRange(new[]
    {
        new KeyValuePair<string, string>(__BindableObjectAttributeEmbeddedResourceName__, __BindableObjectFullAttribute__),
    });

    private static readonly ImmutableDictionary<string, DiagnosticDescriptor> GeneratorAttributeNamesToDiagnosticsMap = ImmutableDictionary.CreateRange(new[]
    {
        new KeyValuePair<string, DiagnosticDescriptor>(__BindableObjectAttributeEmbeddedResourceName__, DiagnosticDescriptors.CreateInheritFromBindableObjectInsteadOfUsingBindableObjectAttributeWarning<BindableObjectSourceGenerator>(__BindableObject__)),
    });

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.CreateInheritFromBindableObjectInsteadOfUsingBindableObjectAttributeWarning<BindableObjectSourceGenerator>(__BindableObject__));

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(static context =>
        {
            if (!context.Compilation.TryBuildNamedTypeSymbolMap(GeneratorAttributeNamesToFullyQualifiedNamesMap, out ImmutableDictionary<string, INamedTypeSymbol>? typeSymbols))
                return;

            context.RegisterSymbolAction(context =>
            {
                if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Class, IsRecord: false, IsStatic: false, IsImplicitlyDeclared: false, BaseType.SpecialType: SpecialType.System_Object } classSymbol)
                    return;

                var baseType = classSymbol.BaseType;
                if (baseType is not null)
                {
                    foreach (AttributeData attribute in context.Symbol.GetAttributes())
                    {
                        if (attribute.AttributeClass is { Name: string attributeName } attributeClass &&
                            typeSymbols.TryGetValue(attributeName, out INamedTypeSymbol? attributeSymbol) &&
                            SymbolEqualityComparer.Default.Equals(attributeClass, attributeSymbol))
                        {
                            if (baseType.ToDisplayString() != __BindableFullObject__ && baseType.ToDisplayString() != __object__)
                            {
#pragma warning disable RS1005 // ReportDiagnostic invoked with an unsupported DiagnosticDescriptor
                                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateDuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError<BindableObjectSourceGenerator>(__BindableObject__),
                                                         context.Symbol.Locations.FirstOrDefault(),
                                                         ImmutableDictionary.Create<string, string?>()
                                                            .Add(TypeNameKey, classSymbol.Name)
                                                            .Add(AttributeTypeNameKey, attributeName),
                                                         context.Symbol));
#pragma warning restore RS1005 // ReportDiagnostic invoked with an unsupported DiagnosticDescriptor
                            }
                        }
                    }
                }
                else
                {
                    foreach (AttributeData attribute in context.Symbol.GetAttributes())
                    {
                        if (attribute.AttributeClass is { Name: string attributeName } attributeClass &&
                            typeSymbols.TryGetValue(attributeName, out INamedTypeSymbol? attributeSymbol) &&
                            SymbolEqualityComparer.Default.Equals(attributeClass, attributeSymbol))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                GeneratorAttributeNamesToDiagnosticsMap[attributeClass.Name],
                                context.Symbol.Locations.FirstOrDefault(),
                                ImmutableDictionary.Create<string, string?>()
                                    .Add(TypeNameKey, classSymbol.Name)
                                    .Add(AttributeTypeNameKey, attributeName),
                                context.Symbol));
                        }
                    }
                }
            }, SymbolKind.NamedType);
        });
    }
}
