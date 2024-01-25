using Prism.SourceGenerators.Extensions;

namespace Prism.SourceGenerators.Diagnostics.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AutoPropertyWithFieldTargetedBindablePropertyAttributeAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.AutoPropertyBackingFieldBindableProperty);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(static context =>
        {
            // Get the symbol for [ObservableProperty]
            if (context.Compilation.GetTypeByMetadataName("Prism.Mvvm.BindablePropertyAttribute") is not INamedTypeSymbol observablePropertySymbol)
                return;

            context.RegisterSymbolAction(context =>
            {
                // Get the property symbol and the type symbol for the containing type
                if (context.Symbol is not IPropertySymbol { ContainingType: INamedTypeSymbol typeSymbol } propertySymbol)
                    return;

                foreach (ISymbol memberSymbol in typeSymbol.GetMembers())
                {
                    // We're only looking for fields with an associated property
                    if (memberSymbol is not IFieldSymbol { AssociatedSymbol: IPropertySymbol associatedPropertySymbol })
                        continue;

                    // Check that this field is in fact the backing field for the target auto-property
                    if (!SymbolEqualityComparer.Default.Equals(associatedPropertySymbol, propertySymbol))
                        continue;

                    // If the field isn't using [ObservableProperty], this analyzer isn't applicable
                    if (!memberSymbol.TryGetAttributeWithType(observablePropertySymbol, out AttributeData? attributeData))
                        return;

                    // Report the diagnostic on the attribute location
                    context.ReportDiagnostic(Diagnostic.Create(
                        DiagnosticDescriptors.AutoPropertyBackingFieldBindableProperty,
                        attributeData.GetLocation(),
                        typeSymbol,
                        propertySymbol));
                }
            }, SymbolKind.Property);
        });
    }
}
