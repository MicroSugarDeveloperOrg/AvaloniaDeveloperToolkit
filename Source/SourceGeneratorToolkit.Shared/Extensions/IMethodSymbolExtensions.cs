namespace SourceGeneratorToolkit.Extensions;

internal static class IMethodSymbolExtensions
{
    public static string GetGeneratedMethodName(this IMethodSymbol methodSymbol)
    {
        string methodName = methodSymbol.Name;
        if (methodName.Length > 2 && methodName.StartsWith("On") && !char.IsLower(methodName[2]))
            return methodName.Substring(2);

        return methodName;
    }

    public static string? CreateParameter(this IMethodSymbol methodSymbol)
    {
        if (methodSymbol is null)
            return default;

        if (methodSymbol.Parameters.Length <= 0)
            return default;

        int i = 0;
        StringBuilder builder = new();
        foreach (var parameter in methodSymbol.Parameters)
        {
            if (i > 0)
                builder.Append(", ");

            builder.Append($"{parameter.Type.Name} {parameter.Name}");
            i++;
        }
          
        return builder.ToString();
    }
}
