using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AvaloniaPropertySourceGenerator.Extensions;
internal static class TypeDeclarationSyntaxExtensions
{
    public static bool HasOrPotentiallyHasAttributes(this TypeDeclarationSyntax typeDeclarationSyntax)
    {
        if (typeDeclarationSyntax.AttributeLists.Count > 0)
            return true;

        foreach (var modifier in typeDeclarationSyntax.Modifiers)
        {
            if (modifier.IsKind(SyntaxKind.PartialKeyword))
                return true;
        }


        return false;
    }

    public static bool HasOrPotentiallyHasBaseTypes(this TypeDeclarationSyntax typeDeclarationSyntax)
    {
        if (typeDeclarationSyntax.BaseList is { Types.Count: > 0 })
            return true;

        foreach (var modifier in typeDeclarationSyntax.Modifiers)
        {
            if (modifier.IsKind(SyntaxKind.PartialKeyword))
                return true;
        }

        return false;
    }

    public static bool IsPartialClass(this ClassDeclarationSyntax typeDeclarationSyntax)
    {
        foreach (var modifier in typeDeclarationSyntax.Modifiers)
        {
            if (modifier.IsKind(SyntaxKind.PartialKeyword))
                return true;
        }

        return false;
    }

}
