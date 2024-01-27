using Prism.SourceGenerators.Generators;

namespace Prism.SourceGenerators.Diagnostics;

#pragma warning disable RS2008 // Enable analyzer release tracking

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor InheritFromBindableObjectInsteadOfUsingBindableObjectAttributeWarning = new DiagnosticDescriptor(
           id: "PRSMSG0011",
           title: "Inherit from BindableObject instead of using [BindableObject]",
           messageFormat: "The type {0} is using the [BindableObject] attribute while having no base type, and it should instead inherit from BindableObject",
           category: typeof(BindableObjectSourceGenerator).FullName,
           defaultSeverity: DiagnosticSeverity.Warning,
           isEnabledByDefault: true,
           description:
               "Classes with no base types should prefer inheriting from BindableObject instead of using attributes to generate INotifyPropertyChanged code, as that will " +
               "reduce the binary size of the application (the attributes are only meant to support cases where the annotated types are already inheriting from a different type).",
           helpLinkUri: "");

    public static readonly DiagnosticDescriptor BindablePropertyNameCollisionError = new(
           id: "PRISMSG0014",
           title: "Name collision for generated property",
           messageFormat: "The field {0}.{1} cannot be used to generate an bindable property, as its name would collide with the field name (instance fields should use the \"lowerCamel\", \"_lowerCamel\" or \"m_lowerCamel\" pattern)",
           category: typeof(BindablePropertySourceGenerator).FullName,
           defaultSeverity: DiagnosticSeverity.Error,
           isEnabledByDefault: true,
           description: "The name of fields annotated with [BindableProperty] should use \"lowerCamel\", \"_lowerCamel\" or \"m_lowerCamel\" pattern to avoid collisions with the generated properties.",
           helpLinkUri: "");

    public static readonly DiagnosticDescriptor BindableCommandNameCollisionError = new(
           id: "PRISMSG0015",
           title: "Name collision for generated command",
           messageFormat: "The command {0}.{1} cannot be used to generate an bindable command, as its name would collide with the method name (instance method should use only one argument and return value is void)",
           category: typeof(BindableCommandSourceGenerator).FullName,
           defaultSeverity: DiagnosticSeverity.Error,
           isEnabledByDefault: true,
           description: "The name of method annotated with [BindableCommand] should use only one argument and no return value.",
           helpLinkUri: "");

    public static readonly DiagnosticDescriptor InvalidBindableCommandMethodSignatureError = new(
           id: "PRISMSG0007",
           title: "Invalid BindableCommand method signature",
           messageFormat: "The method {0}.{1} cannot be used to generate a command property, as its signature isn't compatible with any of the existing bindable command types",
           category: typeof(BindableCommandSourceGenerator).FullName,
           defaultSeverity: DiagnosticSeverity.Error,
           isEnabledByDefault: true,
           description: "Cannot apply [BindableCommand] to methods with a signature that doesn't match any of the existing relay command types.",
           helpLinkUri: "");

    public static readonly DiagnosticDescriptor InvalidCanExecuteMemberNameError = new(
           id: "PRISMSG0009",
           title: "Invalid DelegateCommand.CanExecute member name",
           messageFormat: "The CanExecute name must refer to a valid member, but \"{0}\" has no matches in type {1}",
           category: typeof(BindableCommandSourceGenerator).FullName,
           defaultSeverity: DiagnosticSeverity.Error,
           isEnabledByDefault: true,
           description: "The CanExecute name in [DelegateCommand] must refer to a valid member in its parent type.",
           helpLinkUri: "");


    public static readonly DiagnosticDescriptor AutoPropertyBackingFieldBindableProperty = new(
           id: "PRISMSG0040",
           title: "[BindableProperty] on auto-property backing field",
           messageFormat: "The backing field for property {0}.{1} cannot be annotated with [BindableProperty] (the attribute can only be used directly on fields, and the generator will then handle generating the corresponding property)",
           category: typeof(BindablePropertySourceGenerator).FullName,
           defaultSeverity: DiagnosticSeverity.Error,
           isEnabledByDefault: true,
           description: "The backing fields of auto-properties cannot be annotated with [BindableProperty] (the attribute can only be used directly on fields, and the generator will then handle generating the corresponding property).",
           helpLinkUri: "");


    public static readonly DiagnosticDescriptor DuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError = new(
           id: "PRSMSG0002",
           title: $"Duplicate {nameof(INotifyPropertyChanged)} definition",
           messageFormat: $"Cannot apply [BindableObject] to type {{0}}, as it already declares the {nameof(INotifyPropertyChanged)} interface",
           category: typeof(BindableObjectSourceGenerator).FullName,
           defaultSeverity: DiagnosticSeverity.Error,
           isEnabledByDefault: true,
           description: $"Cannot apply [BindableObject] to a type that already declares the {nameof(INotifyPropertyChanged)} interface.",
           helpLinkUri: "");
}
#pragma warning restore RS2008 // Enable analyzer release tracking

