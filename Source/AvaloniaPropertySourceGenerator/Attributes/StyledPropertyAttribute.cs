namespace AvaloniaPropertySourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StyledPropertyAttribute : Attribute
{
    public StyledPropertyAttribute(string propertyName, string propertyType)
    {
        PropertyName = propertyName;
        PropertyType = propertyType;
    }

    public string PropertyName { get; set; }    
    public string PropertyType { get; set; }
    public string? PropertyDefaultValue { get; set; }
}
