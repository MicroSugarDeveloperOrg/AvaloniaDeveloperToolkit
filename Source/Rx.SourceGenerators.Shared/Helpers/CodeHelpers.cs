namespace Rx.SourceGenerators.Helpers;

internal class CodeHelpers
{
    public const string __RxHeader__ = "ReactiveUI";
 
    public const string __RxObjectAttributeEmbeddedResourceName__ = "RxObjectAttribute";
    public const string __RxObjectFullAttribute__ = $"{__RxHeader__}.{__RxObjectAttributeEmbeddedResourceName__}";
    
    public const string __RxObject__ = $"ReactiveObject";
    public const string __RxObjectFull__ = $"{__RxHeader__}.{__RxObject__}";

    public const string __RxPropertyAttributeEmbeddedResourceName__ = "RxPropertyAttribute";
    public const string __RxPropertyFullAttribute__ = $"{__RxHeader__}.{__RxPropertyAttributeEmbeddedResourceName__}";

    public const string __RxProperty__ = "ReactiveProperty";
    public const string __RxPropertyFull__ = $"{__RxHeader__}.{__RxProperty__}";

    public const string __RxCommandAttributeEmbeddedResourceName__ = "RxCommandAttribute";
    public const string __RxCommandFullAttribute__ = $"{__RxHeader__}.{__RxCommandAttributeEmbeddedResourceName__}";

    public const string __RxCommand__ = "ReactiveCommand";
    public const string __RxCommandFull__ = $"{__RxHeader__}.{__RxCommand__}";

    public const string __GeneratorCSharpFileHeader__ = "_rx_";

}
