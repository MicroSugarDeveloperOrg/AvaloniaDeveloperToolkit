using AvaloniaPropertySourceGenerator.Attributes;
using AvaloniaPropertySourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace AvaloniaPropertySourceGenerator;

[Generator]
//[Generator(LanguageNames.CSharp)]
//#pragma warning disable RS1036 // Specify analyzer banned API enforcement setting
public class StyledPropertySourceGenerator : IIncrementalGenerator
//#pragma warning restore RS1036 // Specify analyzer banned API enforcement setting
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        //Debugger.Launch();
        var classInfo = context.SyntaxProvider.ForAttributeWithMetadataName(typeof(StyledPropertyAttribute).FullName,
            (node, token) => node is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.HasOrPotentiallyHasAttributes() && classDeclarationSyntax.HasOrPotentiallyHasBaseTypes(),
            (context, token) =>
            {
                if (!context.SemanticModel.Compilation.HasLanguageVersionAtLeastEqualTo(LanguageVersion.CSharp8))
                    return default;

                if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
                    return default;

                if (!typeSymbol.HasAccessibleBaseTypeWithMetadataName("AvaloniaObject"))
                    return default;

                return typeSymbol;
            }).Where(item => item is not null);

        context.RegisterSourceOutput(classInfo, (context, symbol) => 
        {



        });

    }
}
