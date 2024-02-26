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

    public static bool IsImplementedOf(this INamedTypeSymbol classSymbol, string interfaceType)
    {
        if (classSymbol is null || string.IsNullOrWhiteSpace(interfaceType))
            return false;

        var classSymbolInner = classSymbol;
        for (; ; )
        {
            if (classSymbolInner is null)
                break;

            var interfaces = classSymbolInner.AllInterfaces;
            if (interfaces.Length <= 0)
                break;

            foreach (var item in interfaces)
            {
                if (item.ToDisplayString() == interfaceType)
                    return true;
            }

            classSymbolInner = classSymbol.BaseType;
        }

        return false;
    }

    public static bool IsImplementedOf<T>(this INamedTypeSymbol classSymbol)
    {
        var interfaceType = typeof(T).Name;
        if (classSymbol is null || string.IsNullOrWhiteSpace(interfaceType))
            return false;

        var classSymbolInner = classSymbol;
        for (; ; )
        {
            if (classSymbolInner is null)
                break;

            var interfaces = classSymbolInner.AllInterfaces;
            if (interfaces.Length <= 0)
                break;

            foreach (var item in interfaces)
            {
                if (item.ToDisplayString() == interfaceType)
                    return true;
            }

            classSymbolInner = classSymbol.BaseType;
        }

        return false;
    }
}