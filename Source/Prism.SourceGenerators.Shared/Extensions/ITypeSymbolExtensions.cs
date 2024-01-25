using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.SourceGenerators.Extensions;
internal static class ITypeSymbolExtensions
{
    public static bool HasFullyQualifiedMetadataName(this ITypeSymbol symbol, string name)
    {
        //using ImmutableArrayBuilder<char> builder = ImmutableArrayBuilder<char>.Rent();

        //symbol.AppendFullyQualifiedMetadataName(in builder);



        //return builder.WrittenSpan.SequenceEqual(name.AsSpan());

        return true;
    }
}
