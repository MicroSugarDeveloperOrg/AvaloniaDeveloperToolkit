namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class ScopedAttribute<TFrom, TTo> : ScopedAttribute where TTo : TFrom
{
    public ScopedAttribute()
        : base(typeof(TFrom), typeof(TTo))
    { }
}
