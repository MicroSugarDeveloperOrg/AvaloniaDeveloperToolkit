namespace Prism.SourceGenerators.Attributes;

#nullable enable

internal class InjectAttribute : Attribute
{
    public InjectAttribute(Type from)
    {
        From = from;
    }

    public InjectAttribute(Type from, Type to)
        : this(from)
    {
        To = to;
    }

    public Type From { get; init; }
    public Type? To { get; init; }
    public string? Token { get; init; } 
}

#nullable disable