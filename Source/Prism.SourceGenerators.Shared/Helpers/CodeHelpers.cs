namespace Prism.SourceGenerators.Helpers;

internal class CodeHelpers
{
    public const string __PrismMvvmHeader__ = "Prism.Mvvm";
    public const string __PrismCommandsHeader__ = "Prism.Commands";
    public const string __PrismIocHeader__ = "Prism.Ioc";

    public const string __BindableObjectAttributeEmbeddedResourceName__ = "BindableObjectAttribute";
    public const string __BindableObjectFullAttribute__ = $"{__PrismMvvmHeader__}.{__BindableObjectAttributeEmbeddedResourceName__}";

    public const string __BindableObject__ = "BindableBase";
    public const string __BindableFullObject__ = $"{__PrismMvvmHeader__}.{__BindableObject__}";

    public const string __BindablePropertyAttributeEmbeddedResourceName__ = "BindablePropertyAttribute";
    public const string __BindablePropertyFullAttribute__ = $"{__PrismMvvmHeader__}.{__BindablePropertyAttributeEmbeddedResourceName__}";
    
    public const string __BindableProperty__ = "BindableProperty";
    public const string __BindablePropertyFull__ = $"{__PrismMvvmHeader__}.{__BindableProperty__}";

    public const string __BindableCommandAttributeEmbeddedResourceName__ = "BindableCommandAttribute";
    public const string __BindableCommandFullAttribute__ = $"{__PrismCommandsHeader__}.{__BindableCommandAttributeEmbeddedResourceName__}";
    
    public const string __BindableCommand__ = "BindableCommand";
    public const string __BindableCommandFull__ = $"{__PrismCommandsHeader__}.{__BindableCommand__}";

    public const string __GeneratorCSharpFileHeader__ = "_Prism_";


}
