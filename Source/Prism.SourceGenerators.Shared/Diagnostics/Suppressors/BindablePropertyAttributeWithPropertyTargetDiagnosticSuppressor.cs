using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using static Prism.SourceGenerators.Helpers.CodeHelpers;

namespace Prism.SourceGenerators.Diagnostics.Suppressors;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class BindablePropertyAttributeWithPropertyTargetDiagnosticSuppressor : DiagnosticSuppressor
{
    public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions { get; } = ImmutableArray.Create(SuppressionDescriptors.CreatePropertyAttributeListForBindablePropertyField(__BindableProperty__));

    public override void ReportSuppressions(SuppressionAnalysisContext context)
    {
        foreach (Diagnostic diagnostic in context.ReportedDiagnostics)
        {
            SyntaxNode? syntaxNode = diagnostic.Location.SourceTree?.GetRoot(context.CancellationToken).FindNode(diagnostic.Location.SourceSpan);

            if (syntaxNode is AttributeTargetSpecifierSyntax { Parent.Parent: FieldDeclarationSyntax { Declaration.Variables.Count: > 0 } fieldDeclaration } attributeTarget &&
                attributeTarget.Identifier.IsKind(SyntaxKind.PropertyKeyword))
            {
                SemanticModel semanticModel = context.GetSemanticModel(syntaxNode.SyntaxTree);

                ISymbol? declaredSymbol = semanticModel.GetDeclaredSymbol(fieldDeclaration.Declaration.Variables[0], context.CancellationToken);

                if (declaredSymbol is IFieldSymbol fieldSymbol &&
                    semanticModel.Compilation.GetTypeByMetadataName(__BindablePropertyFullAttribute__) is INamedTypeSymbol observablePropertySymbol &&
                    fieldSymbol.HasAttributeWithType(observablePropertySymbol))
                {
                    context.ReportSuppression(Suppression.Create(SupportedSuppressions.First(), diagnostic));
                }
            }
        }
    }
}