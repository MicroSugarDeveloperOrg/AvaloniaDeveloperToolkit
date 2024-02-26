using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal sealed class SingletonAttribute<T> : SingletonAttribute
{
    public SingletonAttribute() 
        : base(typeof(T))
    { }
}
