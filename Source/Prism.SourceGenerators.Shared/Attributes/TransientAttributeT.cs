namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class TransientAttribute<T> : TransientAttribute
{
    public TransientAttribute()
        : base(typeof(T))
    { }
}
