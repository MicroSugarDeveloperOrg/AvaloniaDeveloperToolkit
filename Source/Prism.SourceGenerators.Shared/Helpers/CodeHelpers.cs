namespace Prism.SourceGenerators.Helpers;

internal class CodeHelpers
{
    public const string __PrismMvvmHeader__ = "Prism.Mvvm";
    public const string __PrismCommandsHeader__ = "Prism.Commands";
    public const string __PrismIocHeader__ = "Prism.Ioc";

    public const string __PrismContainer__ = "IContainerRegistry";
    public const string __PrismContainerFull__ = $"{__PrismIocHeader__}.{__PrismContainer__}";

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

    public const string __InjectAttributeEmbeddedResourceName__ = "InjectAttribute";
    public const string __InjectFullAttribute__ = $"{__PrismIocHeader__}.{__InjectAttributeEmbeddedResourceName__}";

    public const string __ManyInjectAttributeEmbeddedResourceName__ = "ManyInjectAttribute";
    public const string __ManyInjectFullAttribute__ = $"{__PrismIocHeader__}.{__ManyInjectAttributeEmbeddedResourceName__}";

    public const string __NavigationAttributeEmbeddedResourceName__ = "NavigationAttribute";
    public const string __NavigationAttributeEmbeddedResourceNameT__ = "NavigationAttributeT";
    public const string __NavigationAttributeEmbeddedResourceNameTT__ = "NavigationAttributeTT";
    public const string __NavigationFullAttribute__ = $"{__PrismIocHeader__}.{__NavigationAttributeEmbeddedResourceName__}";

    public const string __Navigation__ = "RegisterForNavigation";

    public const string __ScopedAttributeEmbeddedResourceName__ = "ScopedAttribute";
    public const string __ScopedAttributeEmbeddedResourceNameT__ = "ScopedAttributeT";
    public const string __ScopedAttributeEmbeddedResourceNameTT__ = "ScopedAttributeTT";
    public const string __ScopedFullAttribute__ = $"{__PrismIocHeader__}.{__ScopedAttributeEmbeddedResourceName__}";

    public const string __ManyScopedAttributeEmbeddedResourceName__ = "ManyScopedAttribute";
    public const string __ManyScopedFullAttribute__ = $"{__PrismIocHeader__}.{__ManyScopedAttributeEmbeddedResourceName__}";

    public const string __Scoped__ = "RegisterScoped";
    public const string __ManyScoped__ = "RegisterManyScoped";

    public const string __SingletonAttributeEmbeddedResourceName__ = "SingletonAttribute";
    public const string __SingletonAttributeEmbeddedResourceNameT__ = "SingletonAttributeT";
    public const string __SingletonAttributeEmbeddedResourceNameTT__ = "SingletonAttributeTT";
    public const string __SingletonFullAttribute__ = $"{__PrismIocHeader__}.{__SingletonAttributeEmbeddedResourceName__}";

    public const string __ManySingletonAttributeEmbeddedResourceName__ = "ManySingletonAttribute";
    public const string __ManySingletonFullAttribute__ = $"{__PrismIocHeader__}.{__ManySingletonAttributeEmbeddedResourceName__}";

    public const string __Singleton__ = "RegisterSingleton";
    public const string __ManySingleton__ = "RegisterManySingleton";

    public const string __TransientAttributeEmbeddedResourceName__ = "TransientAttribute";
    public const string __TransientAttributeEmbeddedResourceNameT__ = "TransientAttributeT";
    public const string __TransientAttributeEmbeddedResourceNameTT__ = "TransientAttributeTT";
    public const string __TransientFullAttribute__ = $"{__PrismIocHeader__}.{__TransientAttributeEmbeddedResourceName__}";

    public const string __ManyTransientAttributeEmbeddedResourceName__ = "ManyTransientAttribute";
    public const string __ManyTransientFullAttribute__ = $"{__PrismIocHeader__}.{__ManyTransientAttributeEmbeddedResourceName__}";

    public const string __Transient__ = "Register";
    public const string __ManyTransient__ = "RegisterMany";

    public const string __Registration__ = "Registration";

    public const string __AttributeConstructorParameter_From__ = "from";
    public const string __AttributeConstructorParameter_To__ = "to";
    public const string __AttributeConstructorNamedArgument_From__ = "From";
    public const string __AttributeConstructorNamedArgument_To__ = "To";
    public const string __AttributeConstructorNamedArgument_Token__ = "Token";

    public const string __GeneratorCSharpFileHeader__ = "_Prism_";
}
