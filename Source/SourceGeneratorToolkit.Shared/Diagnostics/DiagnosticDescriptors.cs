using System.ComponentModel;

namespace SourceGeneratorToolkit.Diagnostics;

internal class DiagnosticDescriptors
{
    public static DiagnosticDescriptor CreateDuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError<TSourceGenerator>(string objectName) where TSourceGenerator : ISourceGenerator
    {
        return new DiagnosticDescriptor(
                   id: "MVVMSG0001",
                   title: $"Duplicate {nameof(INotifyPropertyChanged)} definition",
                   messageFormat: $"Cannot apply [{objectName}] to type {{0}}, as it already declares the {nameof(INotifyPropertyChanged)} interface",
                   category: typeof(TSourceGenerator).FullName,
                   defaultSeverity: DiagnosticSeverity.Error,
                   isEnabledByDefault: true,
                   description: $"Cannot apply [{objectName}] to a type that already declares the {nameof(INotifyPropertyChanged)} interface.",
                   helpLinkUri: "");
    }

    public static DiagnosticDescriptor CreateBindablePropertyNameCollisionError<TSourceGenerator>(string propertyName) where TSourceGenerator : ISourceGenerator
    {
        return new DiagnosticDescriptor(
                   id: "MVVMSG0002",
                   title: "Name collision for generated property",
                   messageFormat: "The field {0}.{1} cannot be used to generate an bindable property, as its name would collide with the field name (instance fields should use the \"lowerCamel\", \"_lowerCamel\" or \"m_lowerCamel\" pattern)",
                   category: typeof(TSourceGenerator).FullName,
                   defaultSeverity: DiagnosticSeverity.Error,
                   isEnabledByDefault: true,
                   description: $"The name of fields annotated with [{propertyName}] should use \"lowerCamel\", \"_lowerCamel\" or \"m_lowerCamel\" pattern to avoid collisions with the generated properties.",
                   helpLinkUri: "");
    }

    public static DiagnosticDescriptor CreateAutoPropertyBackingFieldBindableProperty<TSourceGenerator>(string propertyName) where TSourceGenerator : ISourceGenerator
    {
        return new DiagnosticDescriptor(
                   id: "MVVMSG003",
                   title: $"[{propertyName}] on auto-property backing field",
                   messageFormat: $"The backing field for property {0}.{1} cannot be annotated with [{propertyName}] (the attribute can only be used directly on fields, and the generator will then handle generating the corresponding property)",
                   category: typeof(TSourceGenerator).FullName,
                   defaultSeverity: DiagnosticSeverity.Error,
                   isEnabledByDefault: true,
                   description: $"The backing fields of auto-properties cannot be annotated with [{propertyName}] (the attribute can only be used directly on fields, and the generator will then handle generating the corresponding property).",
                   helpLinkUri: "");
    }

    public static DiagnosticDescriptor CreateInvalidBindableCommandMethodSignatureError<TSourceGenerator>(string commandName) where TSourceGenerator : ISourceGenerator
    {
        return new DiagnosticDescriptor(
                   id: "MVVMSG0004",
                   title: $"Invalid {commandName} method signature",
                   messageFormat: "The method {0}.{1} cannot be used to generate a command property, as its signature isn't compatible with any of the existing bindable command types",
                   category: typeof(TSourceGenerator).FullName,
                   defaultSeverity: DiagnosticSeverity.Error,
                   isEnabledByDefault: true,
                   description: $"Cannot apply [{commandName}] to methods with a signature that doesn't match any of the existing relay command types.",
                   helpLinkUri: "");
    }

    public static DiagnosticDescriptor CreateInvalidCanExecuteMemberNameError<TSourceGenerator>(string commandName) where TSourceGenerator : ISourceGenerator
    {
        return new DiagnosticDescriptor(
                   id: "MVVMSG0005",
                   title: $"Invalid {commandName}.CanExecute member name",
                   messageFormat: "The CanExecute name must refer to a valid member, but \"{0}\" has no matches in type {1}",
                   category: typeof(TSourceGenerator).FullName,
                   defaultSeverity: DiagnosticSeverity.Error,
                   isEnabledByDefault: true,
                   description: $"The CanExecute name in [{commandName}] must refer to a valid member in its parent type.",
                   helpLinkUri: "");
    }

    public static DiagnosticDescriptor CreateInheritFromBindableObjectInsteadOfUsingBindableObjectAttributeWarning<TSourceGenerator>(string objectName) where TSourceGenerator : ISourceGenerator
    {
        return new DiagnosticDescriptor(
                   id: "MVVMSG0011",
                   title: $"Inherit from {objectName} instead of using [{objectName}]",
                   messageFormat: $"The type {{0}} is using the [{objectName}] attribute while having no base type, and it should instead inherit from {objectName}",
                   category: typeof(TSourceGenerator).FullName,
                   defaultSeverity: DiagnosticSeverity.Warning,
                   isEnabledByDefault: true,
                   description:
                       $"Classes with no base types should prefer inheriting from {objectName} instead of using attributes to generate INotifyPropertyChanged code, as that will " +
                       "reduce the binary size of the application (the attributes are only meant to support cases where the annotated types are already inheriting from a different type).",
                   helpLinkUri: "");
    }

    public static DiagnosticDescriptor CreateInvalidIocPartialMethodSignatureError<TSourceGenerator>(string iocName, string containerName) where TSourceGenerator : ISourceGenerator
    {
        return new DiagnosticDescriptor(
                   id: "MVVMSG0021",
                   title: $"Invalid {iocName} method signature",
                   messageFormat: $"The Method {{0}}.{{1}} is using the [{iocName}] attribute while having no type {containerName} parameter, and it's parameters should include the type {containerName}",
                   category: typeof(TSourceGenerator).FullName,
                   defaultSeverity: DiagnosticSeverity.Error,
                   isEnabledByDefault: true,
                   description: $"Method's parameters must include the type {containerName}" ,
                   helpLinkUri: "");
    }




}
