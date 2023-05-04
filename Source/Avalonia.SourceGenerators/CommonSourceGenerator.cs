using AvaloniaPropertySourceGenerator.Extensions;
using AvaloniaPropertySourceGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Avalonia.SourceGenerators;
internal abstract class CommonSourceGenerator<TValue> : IIncrementalGenerator
{
    public CommonSourceGenerator(string fullyQualifiedMetadataName, string? baseTypeName = default, LanguageVersion latestLanguageVersion = LanguageVersion.CSharp8)
    {
        _fullyQualifiedMetadataName = fullyQualifiedMetadataName;
        _baseTypeName = baseTypeName;
        _latestLanguageVersion = latestLanguageVersion;
    }

    readonly string _fullyQualifiedMetadataName;
    readonly string? _baseTypeName;
    readonly LanguageVersion _latestLanguageVersion;

    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        var containerProvider = context.SyntaxProvider.ForAttributeWithMetadataName(_fullyQualifiedMetadataName,
            static (node, token) => node is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.HasOrPotentiallyHasBaseTypes(),
            (context, token) =>
            {
                if (!context.SemanticModel.Compilation.HasLanguageVersionAtLeastEqualTo(_latestLanguageVersion))
                    return default;

                if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
                    return default;

                if (!string.IsNullOrWhiteSpace(_baseTypeName))
                {
                    if (!typeSymbol.HasAccessibleBaseTypeWithMetadataName(_baseTypeName!))
                        return default;
                }

                SyntaxHierarchy syntax = SyntaxHierarchy.From(typeSymbol);
                SyntaxMetadata metadata = new(typeSymbol.IsSealed, context.SemanticModel.Compilation.IsNullabilitySupported());

                return new SyntaxContainer<(SyntaxHierarchy syntax, SyntaxMetadata metadata)>((syntax, metadata));
            }).Where(item => item is not null);


    }
}
