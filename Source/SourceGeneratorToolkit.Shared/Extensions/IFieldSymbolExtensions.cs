namespace SourceGeneratorToolkit.Extensions;

internal static class IFieldSymbolExtensions
{
    public static string CreateGeneratedPropertyName(this IFieldSymbol fieldSymbol)
    {
        string propertyName = fieldSymbol.Name;
        if (propertyName.StartsWith("m_"))
            propertyName = propertyName.Substring(2);
        else if (propertyName.StartsWith("_"))
            propertyName = propertyName.TrimStart('_');

        return $"{char.ToUpper(propertyName[0], CultureInfo.InvariantCulture)}{propertyName.Substring(1)}";
    }
}
