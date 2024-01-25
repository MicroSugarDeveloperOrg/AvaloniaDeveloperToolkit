namespace Prism.SourceGenerators.Extensions;
internal static class AttributeDataExtensions
{
    public static Location? GetLocation(this AttributeData attributeData)
    {
        if (attributeData.ApplicationSyntaxReference is { } syntaxReference)
            return syntaxReference.SyntaxTree.GetLocation(syntaxReference.Span);

        return null;
    }
}
