namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal class TransientAttribute<T> : TransientAttribute
{
    public TransientAttribute()
        : base(typeof(T))
    { }
}
