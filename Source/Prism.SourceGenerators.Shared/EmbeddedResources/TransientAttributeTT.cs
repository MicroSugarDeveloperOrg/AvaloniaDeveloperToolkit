using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class TransientAttribute<TFrom, TTo> : TransientAttribute where TTo : TFrom
{
    public TransientAttribute()
        : base(typeof(TFrom), typeof(TTo))
    { }
}
