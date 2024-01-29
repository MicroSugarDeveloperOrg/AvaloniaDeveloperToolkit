namespace SourceGeneratorToolkit.Extensions;

internal static class AttributeDataExtensions
{
    public static Location? GetLocation(this AttributeData attributeData)
    {
        if (attributeData.ApplicationSyntaxReference is { } syntaxReference)
            return syntaxReference.SyntaxTree.GetLocation(syntaxReference.Span);

        return null;
    }

    public static bool TryGetNamedArgument<T>(this AttributeData attributeData, string name, out T? value)
    {
        foreach (KeyValuePair<string, TypedConstant> properties in attributeData.NamedArguments)
        {
            if (properties.Key == name)
            {
                value = (T?)properties.Value.Value;
                return true;
            }
        }

        value = default;
        return false;
    }


}