namespace SourceGeneratorToolkit.Diagnostics;

internal static class SuppressionDescriptors
{
    public static SuppressionDescriptor CreatePropertyAttributeListForBindablePropertyField(string propertyName) => new SuppressionDescriptor(
           id: "MVVMSPR0001",
           suppressedDiagnosticId: "CS0657",
           justification: $"Fields using [{propertyName}] can use [property:] attribute lists to forward attributes to the generated properties");

    public static SuppressionDescriptor CreateFieldOrPropertyAttributeListForBindableCommandMethod(string methodName) => new(
           id: "MVVMSPR0002",
           suppressedDiagnosticId: "CS0657",
           justification: $"Methods using [{methodName}] can use [field:] and [property:] attribute lists to forward attributes to the generated fields and properties");
}