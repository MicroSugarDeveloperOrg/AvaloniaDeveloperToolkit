using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class SingletonAttribute<TFrom,TTo> : SingletonAttribute where TTo : TFrom
{
    public SingletonAttribute() 
        : base(typeof(TFrom), typeof(TTo))
    { }
}
