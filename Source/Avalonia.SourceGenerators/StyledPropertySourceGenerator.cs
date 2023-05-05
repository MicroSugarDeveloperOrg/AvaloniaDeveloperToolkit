using AvaloniaPropertySourceGenerator.Attributes;
using AvaloniaPropertySourceGenerator.Extensions;
using AvaloniaPropertySourceGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace AvaloniaPropertySourceGenerator;

[Generator]
//[Generator(LanguageNames.CSharp)]
//#pragma warning disable RS1036 // Specify analyzer banned API enforcement setting
internal class StyledPropertySourceGenerator : IIncrementalGenerator
//#pragma warning restore RS1036 // Specify analyzer banned API enforcement setting
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        //Debugger.Launch();
        var containerProvider = context.SyntaxProvider.ForAttributeWithMetadataName(typeof(StyledPropertyAttribute).FullName,
            (node, token) => node is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.HasOrPotentiallyHasAttributes() && classDeclarationSyntax.HasOrPotentiallyHasBaseTypes() && classDeclarationSyntax.IsPartialClass(),
            (context, token) =>
            {
                //Debugger.Launch();
                if (!context.SemanticModel.Compilation.HasLanguageVersionAtLeastEqualTo(LanguageVersion.CSharp8))
                    return default;

                if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
                    return default;

                //Debugger.Launch();
                var node = context.TargetNode;

                if (!typeSymbol.HasAccessibleBaseTypeWithMetadataName("AvaloniaObject"))
                    return default;

                //Debugger.Launch();

                SyntaxHierarchy syntax = SyntaxHierarchy.From(typeSymbol);
                SyntaxMetadata metadata = new(typeSymbol.IsSealed, context.SemanticModel.Compilation.IsNullabilitySupported());

                return new SyntaxContainer<(SyntaxHierarchy syntax, SyntaxMetadata metadata,SyntaxNode? node)>((syntax, metadata, node));
            }).Where(item => item is not null);

        context.RegisterSourceOutput(containerProvider, (context, container) =>
        {
            if (container is null)
                return; 

            //CompilationUnitSyntax compilationUnit = container.Value.syntax.CompilationUnitSyntax(updatedMemberDeclarations);

            context.AddSource($"{container.Value.syntax.FilenameHint}.g.cs", "");
            //Debugger.Launch();
        });

    }
}
