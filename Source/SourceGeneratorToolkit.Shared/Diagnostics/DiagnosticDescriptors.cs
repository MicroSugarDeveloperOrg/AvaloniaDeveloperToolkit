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

    public static DiagnosticDescriptor CreateInvalidBindableCommandMethodSignatureError<TSourceGenerator>(string commandName) where TSourceGenerator : ISourceGenerator
    {
        return new DiagnosticDescriptor(
                   id: "MVVMSG0003",
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
                   id: "MVVMSG0004",
                   title: $"Invalid {commandName}.CanExecute member name",
                   messageFormat: "The CanExecute name must refer to a valid member, but \"{0}\" has no matches in type {1}",
                   category: typeof(TSourceGenerator).FullName,
                   defaultSeverity: DiagnosticSeverity.Error,
                   isEnabledByDefault: true,
                   description: $"The CanExecute name in [{commandName}] must refer to a valid member in its parent type.",
                   helpLinkUri: "");
    }

}
