using Prism.SourceGenerators.Generators;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using static Prism.SourceGenerators.Helpers.CodeHelpers;

namespace Prism.SourceGenerators.Diagnostics.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AutoPropertyWithFieldTargetedBindablePropertyAttributeAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.CreateAutoPropertyBackingFieldBindableProperty<BindablePropertySourceGenerator>(__BindableProperty__));

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(static context =>
        {
            if (context.Compilation.GetTypeByMetadataName(__BindablePropertyFullAttribute__) is not INamedTypeSymbol observablePropertySymbol)
                return;

            context.RegisterSymbolAction(context =>
            {
                if (context.Symbol is not IPropertySymbol { ContainingType: INamedTypeSymbol typeSymbol } propertySymbol)
                    return;

                foreach (ISymbol memberSymbol in typeSymbol.GetMembers())
                {
                    if (memberSymbol is not IFieldSymbol { AssociatedSymbol: IPropertySymbol associatedPropertySymbol })
                        continue;

                    if (!SymbolEqualityComparer.Default.Equals(associatedPropertySymbol, propertySymbol))
                        continue;

                    if (!memberSymbol.TryGetAttributeWithType(observablePropertySymbol, out AttributeData? attributeData))
                        return;

                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateAutoPropertyBackingFieldBindableProperty<BindablePropertySourceGenerator>(__BindableProperty__),
                                             attributeData.GetLocation(),
                                             typeSymbol,
                                             propertySymbol));
                }
            }, SymbolKind.Property);
        });
    }
}
