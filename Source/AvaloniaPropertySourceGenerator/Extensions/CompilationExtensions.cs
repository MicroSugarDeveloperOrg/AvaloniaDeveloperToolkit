using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AvaloniaPropertySourceGenerator.Extensions;
internal static class CompilationExtensions
{
    public static bool HasLanguageVersionAtLeastEqualTo(this Compilation compilation, LanguageVersion languageVersion)
    {
        return ((CSharpCompilation)compilation).LanguageVersion >= languageVersion;
    }

    public static bool HasAccessibleBaseTypeWithMetadataName(this INamedTypeSymbol namedTypeSymbol, string baseTypeName)
    {
        if (namedTypeSymbol.BaseType is null)
            return false;

        if (namedTypeSymbol.BaseType.Name == baseTypeName)
            return true;

        return namedTypeSymbol.BaseType.HasAccessibleBaseTypeWithMetadataName(baseTypeName);
    }

    public static bool HasAccessibleAttributeWithMetadataName(this INamedTypeSymbol namedTypeSymbol, string attribute)
    {
        return true;
    }

    //public static bool HasAccessibleTypeWithMetadataName(this Compilation compilation, string fullyQualifiedMetadataName)
    //{
    //    // Try to get the unique type with this name
    //    INamedTypeSymbol? type = compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);

    //    // If there is only a single matching symbol, check its accessibility
    //    if (type is not null)
    //    {
    //        return type.CanBeAccessedFrom(compilation.Assembly);
    //    }

    //    // Otherwise, try to get the unique type with this name originally defined in 'compilation'
    //    type ??= compilation.Assembly.GetTypeByMetadataName(fullyQualifiedMetadataName);

    //    if (type is not null)
    //    {
    //        return type.CanBeAccessedFrom(compilation.Assembly);
    //    }

    //    // Otherwise, check whether the type is defined and accessible from any of the referenced assemblies
    //    foreach (IModuleSymbol module in compilation.Assembly.Modules)
    //    {
    //        foreach (IAssemblySymbol referencedAssembly in module.ReferencedAssemblySymbols)
    //        {
    //            if (referencedAssembly.GetTypeByMetadataName(fullyQualifiedMetadataName) is not INamedTypeSymbol currentType)
    //            {
    //                continue;
    //            }

    //            switch (currentType.GetEffectiveAccessibility())
    //            {
    //                case Accessibility.Public:
    //                case Accessibility.Internal when referencedAssembly.GivesAccessTo(compilation.Assembly):
    //                    return true;
    //                default:
    //                    continue;
    //            }
    //        }
    //    }

    //    return false;
    //}

}
