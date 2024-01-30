namespace Prism.SourceGenerators.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class ScopedAttribute : InjectAttribute
{
    public ScopedAttribute(Type from)
        : base(from)
    { }

    public ScopedAttribute(Type from, Type to)
        :base(from, to) 
    { }
}
