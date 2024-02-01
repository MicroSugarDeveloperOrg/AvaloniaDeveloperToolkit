namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal class ScopedAttribute : InjectAttribute
{
    public ScopedAttribute(Type to)
        : base(to)
    { }

    public ScopedAttribute(Type from, Type to)
        :base(from, to) 
    { }
}
