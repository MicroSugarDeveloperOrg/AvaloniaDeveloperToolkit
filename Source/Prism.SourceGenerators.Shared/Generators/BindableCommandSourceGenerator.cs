using Prism.SourceGenerators.Extensions;

namespace Prism.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class BindableCommandSourceGenerator : ISourceGenerator
{
    const string __bindableCommandAttributeEmbeddedResourceName__ = "BindableCommandAttribute";

    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {  
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(__bindableCommandAttributeEmbeddedResourceName__));
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
    }

}
