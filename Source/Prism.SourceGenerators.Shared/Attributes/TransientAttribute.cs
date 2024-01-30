namespace Prism.SourceGenerators.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class TransientAttribute : InjectAttribute
{
    public TransientAttribute(Type from)
        : base(from)
    { }

    public TransientAttribute(Type from, Type to)
        : base(from, to)
    { }
}
