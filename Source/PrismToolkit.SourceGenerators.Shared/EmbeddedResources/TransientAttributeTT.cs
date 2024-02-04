using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal sealed class TransientAttribute<TFrom, TTo> : TransientAttribute where TTo : TFrom
{
    public TransientAttribute()
        : base(typeof(TFrom), typeof(TTo))
    { }
}
