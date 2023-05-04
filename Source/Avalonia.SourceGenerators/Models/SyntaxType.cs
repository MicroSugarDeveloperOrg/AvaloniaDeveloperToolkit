using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AvaloniaPropertySourceGenerator.Models;
internal record SyntaxType(string QualifiedName, TypeKind TypeKind, bool IsRecord)
{
    public TypeDeclarationSyntax ToSyntax()
    {
        return TypeKind switch
        {
            TypeKind.Struct => StructDeclaration(QualifiedName),
            TypeKind.Interface => InterfaceDeclaration(QualifiedName),
            TypeKind.Class when IsRecord => RecordDeclaration(Token(SyntaxKind.RecordKeyword), QualifiedName)
                                            .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
                                            .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken)),
            _ => ClassDeclaration(QualifiedName)
        };
    }
}
