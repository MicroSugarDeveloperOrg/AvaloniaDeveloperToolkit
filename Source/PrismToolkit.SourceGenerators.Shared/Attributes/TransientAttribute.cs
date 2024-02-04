namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal class TransientAttribute : InjectAttribute
{
    public TransientAttribute(Type to)
        : base(to)
    { }

    public TransientAttribute(Type from, Type to)
        : base(from, to)
    { }
}
