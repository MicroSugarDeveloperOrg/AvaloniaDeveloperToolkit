namespace Prism.SourceGenerators.Helpers;
internal class CodeHelpers
{
    public const string __PrismMvvmHeader__ = "Prism.Mvvm";
    public const string __PrismCommandsHeader__ = "Prism.Commands";
    public const string __EmbeddedResourcesHeader__ = "Prism.SourceGenerators.EmbeddedResources";

    public const string __BindableObjectAttributeEmbeddedResourceName__ = "BindableObjectAttribute";
    public const string __BindableObjectEmbeddedResourceName__ = "BindableBase";
    //public const string __BindableObjectEmbeddedResourceName__ = "BindableObject";

    public const string __BindableObjectAttribute__ = $"{__PrismMvvmHeader__}.{__BindableObjectAttributeEmbeddedResourceName__}";
    public const string __BindableObject__ = $"{__PrismMvvmHeader__}.{__BindableObjectEmbeddedResourceName__}";

    public const string __BindablePropertyAttributeEmbeddedResourceName__ = "BindablePropertyAttribute";
    public const string __BindablePropertyAttribute__ = $"{__PrismMvvmHeader__}.{__BindablePropertyAttributeEmbeddedResourceName__}";
    public const string __BindableProperty__ = "BindableProperty";

    public const string __BindableCommandAttributeEmbeddedResourceName__ = "BindableCommandAttribute";
    public const string __BindableCommandAttribute__ = $"{__PrismCommandsHeader__}.{__BindableCommandAttributeEmbeddedResourceName__}";
    public const string __BindableCommand__ = "BindableCommand";

    public const string __CSharpFileExtension__ = "cs";
    public const string __GeneratorCSharpFileHeader__ = "_Prism_";
    public const string __GeneratorCSharpFileExtension__ = $"g.{__CSharpFileExtension__}";

    public const string __object__ = "object";

}
