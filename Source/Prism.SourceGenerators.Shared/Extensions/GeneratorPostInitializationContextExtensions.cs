using System.IO;
using System.Reflection;

namespace Prism.SourceGenerators.Extensions;
internal static class GeneratorPostInitializationContextExtensions
{
    const string __fileHeader__ = "prism_";
    const string __fileTail__ = ".g.cs";

    public static bool CreateSourceCodeFromEmbeddedResource(this GeneratorPostInitializationContext context, string resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName))
            return false;

        var embeddedResource = $"Prism.SourceGenerators.EmbeddedResources.{resourceName}.cs";
        var targetFile = $"{__fileHeader__}{resourceName}{__fileTail__}";
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResource);
        if (stream is null)
            return false;

        using StreamReader reader = new(stream);
        string sourceCode = reader.ReadToEnd();
        context.AddSource(targetFile, sourceCode);

        return true;
    }
}
