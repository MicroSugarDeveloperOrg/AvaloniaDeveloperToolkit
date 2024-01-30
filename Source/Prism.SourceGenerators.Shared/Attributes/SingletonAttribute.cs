namespace Prism.SourceGenerators.Attributes;

#nullable enable

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class SingletonAttribute : InjectAttribute
{
    public SingletonAttribute(Type from) 
        : base(from)
    { }

    public SingletonAttribute(Type from, Type to) 
        : base(from, to)
    { }
}

#nullable disable
