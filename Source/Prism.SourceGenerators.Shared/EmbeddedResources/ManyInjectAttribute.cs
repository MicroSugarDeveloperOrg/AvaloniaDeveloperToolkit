namespace Prism.Ioc;

#nullable enable

internal class ManyInjectAttribute : Attribute
{
    public ManyInjectAttribute(Type type, params Type[] serviceTypes)
    {
        Type = type;
        ServiceTypes = serviceTypes;
    }

    public Type Type { get; init; }

    public Type[] ServiceTypes { get; init;}

    public string? Token { get; init; } 
}

#nullable disable