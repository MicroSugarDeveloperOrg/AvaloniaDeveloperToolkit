using Prism.SourceGenerators.Extensions;

namespace Prism.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class BindablePropertySourceGenerator : ISourceGenerator
{
    const string __bindablePropertyAttributeEmbeddedResourceName__ = "BindablePropertyAttribute"; 

    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(__bindablePropertyAttributeEmbeddedResourceName__));
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {

    }

}
