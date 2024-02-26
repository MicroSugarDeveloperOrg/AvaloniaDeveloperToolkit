using Prism.SourceGenerators.Diagnostics.Analyzers;

namespace Prism.SourceGenerators.CodeFixers;

[ExportCodeFixProvider(LanguageNames.CSharp)]
[Shared]
public sealed class ClassUsingAttributeInsteadOfInheritanceCodeFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create("PRSMSG0011");

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        Diagnostic diagnostic = context.Diagnostics[0];
        TextSpan diagnosticSpan = context.Span;

        if (diagnostic.Properties[ClassUsingAttributeInsteadOfInheritanceAnalyzer.TypeNameKey] is not string typeName ||
            diagnostic.Properties[ClassUsingAttributeInsteadOfInheritanceAnalyzer.AttributeTypeNameKey] is not string attributeTypeName)
            return;

        SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        if (root!.FindNode(diagnosticSpan) is ClassDeclarationSyntax { Identifier.Text: string identifierName } classDeclaration &&
            identifierName == typeName)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Inherit from BindableObject",
                    createChangedDocument: token => RemoveAttribute(context.Document, root, classDeclaration, attributeTypeName),
                    equivalenceKey: "Inherit from BindableObject"),
                diagnostic);
        }
    }

    public override FixAllProvider? GetFixAllProvider()
    {
        return base.GetFixAllProvider();
    }

    private static Task<Document> RemoveAttribute(Document document, SyntaxNode root, ClassDeclarationSyntax classDeclaration, string attributeTypeName)
    {
        SyntaxGenerator generator = SyntaxGenerator.GetGenerator(document);
        ClassDeclarationSyntax updatedClassDeclaration = (ClassDeclarationSyntax)generator.AddBaseType(classDeclaration, SyntaxFactory.IdentifierName("BindableObject"));

        foreach (AttributeListSyntax attributeList in updatedClassDeclaration.AttributeLists)
        {
            foreach (AttributeSyntax attribute in attributeList.Attributes)
            {
                if (attribute.Name is IdentifierNameSyntax { Identifier.Text: string identifierName } &&
                    (identifierName == attributeTypeName || (identifierName + "Attribute") == attributeTypeName))
                {
                    updatedClassDeclaration = (ClassDeclarationSyntax)generator.RemoveNode(updatedClassDeclaration, attribute);
                    break;
                }
            }
        }

        return Task.FromResult(document.WithSyntaxRoot(root.ReplaceNode(classDeclaration, updatedClassDeclaration)));
    }
}
