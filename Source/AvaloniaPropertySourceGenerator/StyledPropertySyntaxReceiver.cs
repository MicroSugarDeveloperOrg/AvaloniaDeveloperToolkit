using AvaloniaPropertySourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AvaloniaPropertySourceGenerator;
internal class StyledPropertySyntaxReceiver : ISyntaxReceiver
{
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.HasOrPotentiallyHasAttributes())
        {

        }
    }
}
