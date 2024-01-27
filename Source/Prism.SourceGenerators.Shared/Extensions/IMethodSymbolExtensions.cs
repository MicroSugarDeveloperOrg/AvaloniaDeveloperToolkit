namespace Prism.SourceGenerators.Extensions;

internal static class IMethodSymbolExtensions
{
    public static string GetGeneratedMethodName(this IMethodSymbol methodSymbol)
    {
        string methodName = methodSymbol.Name;
        if (methodName.Length > 2 && methodName.StartsWith("On") && !char.IsLower(methodName[2]))
            return methodName.Substring(2);
         
        return methodName;
    }
}
