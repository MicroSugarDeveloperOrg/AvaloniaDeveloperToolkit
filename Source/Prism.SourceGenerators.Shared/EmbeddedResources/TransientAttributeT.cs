using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal sealed class TransientAttribute<T> : TransientAttribute
{
    public TransientAttribute()
        : base(typeof(T))
    { }
}
