using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal sealed class ScopedAttribute<TFrom, TTo> : ScopedAttribute where TTo : TFrom
{
    public ScopedAttribute()
        : base(typeof(TFrom), typeof(TTo))
    { }
}
