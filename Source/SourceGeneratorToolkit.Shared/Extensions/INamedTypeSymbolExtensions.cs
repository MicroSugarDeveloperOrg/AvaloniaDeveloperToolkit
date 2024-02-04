namespace SourceGeneratorToolkit.Extensions;

internal static class INamedTypeSymbolExtensions
{
    public static IEnumerable<ISymbol> GetAllMembers(this INamedTypeSymbol symbol)
    {
        for (INamedTypeSymbol? currentSymbol = symbol; currentSymbol is { SpecialType: not SpecialType.System_Object }; currentSymbol = currentSymbol.BaseType)
        {
            foreach (ISymbol memberSymbol in currentSymbol.GetMembers())
                yield return memberSymbol;
        }
    }

    public static IEnumerable<ISymbol> GetAllMembers(this INamedTypeSymbol symbol, string name)
    {
        for (INamedTypeSymbol? currentSymbol = symbol; currentSymbol is { SpecialType: not SpecialType.System_Object }; currentSymbol = currentSymbol.BaseType)
        {
            foreach (ISymbol memberSymbol in currentSymbol.GetMembers(name))
                yield return memberSymbol;
        }
    }

    public static bool IsBaseOf(this INamedTypeSymbol classSymbol, string baseType)
    {
        if (classSymbol is null || string.IsNullOrWhiteSpace(baseType))
            return false;

        var classSymbolInner = classSymbol;

        for (; ; )
        {
            var baseSymbol = classSymbolInner.BaseType;
            if (baseSymbol is null)
                break;

            if (baseSymbol.ToDisplayString() == baseType)
                return true;

            classSymbolInner = baseSymbol;
        }

        return false;
    }
}