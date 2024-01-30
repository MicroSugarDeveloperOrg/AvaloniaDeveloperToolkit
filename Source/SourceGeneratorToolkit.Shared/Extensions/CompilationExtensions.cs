namespace SourceGeneratorToolkit.Extensions;

internal static class CompilationExtensions
{
    public static bool TryBuildNamedTypeSymbolMap<T>(
       this Compilation compilation,
       IEnumerable<KeyValuePair<T, string>> typeNames,
       [NotNullWhen(true)] out ImmutableDictionary<T, INamedTypeSymbol>? typeSymbols)
       where T : IEquatable<T>
    {
        ImmutableDictionary<T, INamedTypeSymbol>.Builder builder = ImmutableDictionary.CreateBuilder<T, INamedTypeSymbol>();

        foreach (KeyValuePair<T, string> pair in typeNames)
        {
            if (compilation.GetTypeByMetadataName(pair.Value) is not INamedTypeSymbol attributeSymbol)
            {
                typeSymbols = null;

                return false;
            }

            builder.Add(pair.Key, attributeSymbol);
        }

        typeSymbols = builder.ToImmutable();

        return true;
    }
}
