using static SourceGeneratorToolkit.Helpers.CommonHelpers;

namespace SourceGeneratorToolkit.Extensions;

internal static class GeneratorPostInitializationContextExtensions
{
    public static bool CreateSourceCodeFromEmbeddedResource(this GeneratorPostInitializationContext context, string resourceName, string targetHeader)
    {
        if (string.IsNullOrWhiteSpace(resourceName))
            return false;

        var assembly = Assembly.GetExecutingAssembly();
        var header = assembly.GetName().Name;

        var embeddedResource = $"{header}.{__EmbeddedResourcesHeader__}.{resourceName}.{__CSharpFileExtension__}";
        var targetFile = $"{targetHeader}{resourceName}.{__GeneratorCSharpFileExtension__}";
        using Stream stream = assembly.GetManifestResourceStream(embeddedResource);
        if (stream is null)
            return false;

        using StreamReader reader = new(stream);
        string sourceCode = reader.ReadToEnd();
        context.AddSource(targetFile, sourceCode);

        return true;
    }
}
