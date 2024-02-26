namespace Mvvm.SourceGenerators.Helpers;

internal class CodeHelpers
{
    public const string __MvvmHeader__ = "MicroSugar.Mvvm";

    public const string __NotifyPropertyChangedAttributeEmbeddedResourceName__ = "NotifyPropertyChangedAttribute";
    public const string __NotifyPropertyChangedFullAttribute__ = $"{__MvvmHeader__}.{__NotifyPropertyChangedAttributeEmbeddedResourceName__}";

    public const string __NotifyPropertyChangingAttributeEmbeddedResourceName__ = "NotifyPropertyChangingAttribute";
    public const string __NotifyPropertyChangingFullAttribute__ = $"{__MvvmHeader__}.{__NotifyPropertyChangingAttributeEmbeddedResourceName__}";

    public const string __BindablePropertyAttributeEmbeddedResourceName__ = "AutoPropertyAttribute";
    public const string __BindablePropertyFullAttribute__ = $"{__MvvmHeader__}.{__BindablePropertyAttributeEmbeddedResourceName__}";

    public const string __BindableProperty__ = "AutoProperty";
    public const string __BindablePropertyFull__ = $"{__MvvmHeader__}.{__BindableProperty__}";

    public const string __GeneratorCSharpFileHeader__ = "_Mvvm_";
}
