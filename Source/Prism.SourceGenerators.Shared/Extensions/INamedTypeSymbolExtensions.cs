using System.Collections.Generic;

namespace Prism.SourceGenerators.Extensions;
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
}