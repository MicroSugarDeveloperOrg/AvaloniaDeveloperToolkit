using static Prism.SourceGenerators.Helpers.CodeHelpers;

namespace Prism.SourceGenerators.Extensions;
internal static class GeneratorPostInitializationContextExtensions
{
    public static bool CreateSourceCodeFromEmbeddedResource(this GeneratorPostInitializationContext context, string resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName))
            return false;

        var embeddedResource = $"{__EmbeddedResourcesHeader__}.{resourceName}.{__CSharpFileExtension__}";
        var targetFile = $"{__GeneratorCSharpFileHeader__}{resourceName}.{__GeneratorCSharpFileExtension__}";
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResource);
        if (stream is null)
            return false;

        using StreamReader reader = new(stream);
        string sourceCode = reader.ReadToEnd();
        context.AddSource(targetFile, sourceCode);

        return true;
    }
}
