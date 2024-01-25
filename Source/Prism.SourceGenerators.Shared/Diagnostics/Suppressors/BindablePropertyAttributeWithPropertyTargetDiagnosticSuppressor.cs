using Prism.SourceGenerators.Extensions;

namespace Prism.SourceGenerators.Diagnostics.Suppressors;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class BindablePropertyAttributeWithPropertyTargetDiagnosticSuppressor : DiagnosticSuppressor
{
    public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions { get; } = ImmutableArray.Create(SuppressionDescriptors.PropertyAttributeListForBindablePropertyField);

    public override void ReportSuppressions(SuppressionAnalysisContext context)
    {
        foreach (Diagnostic diagnostic in context.ReportedDiagnostics)
        {
            SyntaxNode? syntaxNode = diagnostic.Location.SourceTree?.GetRoot(context.CancellationToken).FindNode(diagnostic.Location.SourceSpan);

            // Check that the target is effectively [property:] over a field declaration with at least one variable, which is the only case we are interested in
            if (syntaxNode is AttributeTargetSpecifierSyntax { Parent.Parent: FieldDeclarationSyntax { Declaration.Variables.Count: > 0 } fieldDeclaration } attributeTarget &&
                attributeTarget.Identifier.IsKind(SyntaxKind.PropertyKeyword))
            {
                SemanticModel semanticModel = context.GetSemanticModel(syntaxNode.SyntaxTree);

                // Get the field symbol from the first variable declaration
                ISymbol? declaredSymbol = semanticModel.GetDeclaredSymbol(fieldDeclaration.Declaration.Variables[0], context.CancellationToken);

                // Check if the field is using [BindableProperty], in which case we should suppress the warning
                if (declaredSymbol is IFieldSymbol fieldSymbol &&
                    semanticModel.Compilation.GetTypeByMetadataName("Prism.Mvvm.BindablePropertyAttribute") is INamedTypeSymbol observablePropertySymbol &&
                    fieldSymbol.HasAttributeWithType(observablePropertySymbol))
                {
                    context.ReportSuppression(Suppression.Create(SuppressionDescriptors.PropertyAttributeListForBindablePropertyField, diagnostic));
                }
            }
        }
    }
}
