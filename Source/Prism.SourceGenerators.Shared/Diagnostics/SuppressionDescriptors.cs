namespace Prism.SourceGenerators.Diagnostics;

internal static class SuppressionDescriptors
{
    public static readonly SuppressionDescriptor PropertyAttributeListForBindablePropertyField = new(
           id: "PRISMSPR0001",
           suppressedDiagnosticId: "CS0657",
           justification: "Fields using [BindableProperty] can use [property:] attribute lists to forward attributes to the generated properties");

    public static readonly SuppressionDescriptor FieldOrPropertyAttributeListForBindableCommandMethod = new(
           id: "PRISMSPR0002",
           suppressedDiagnosticId: "CS0657",
           justification: "Methods using [BindableCommand] can use [field:] and [property:] attribute lists to forward attributes to the generated fields and properties");
}
