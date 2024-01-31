using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class SingletonAttribute<T> : SingletonAttribute
{
    public SingletonAttribute() 
        : base(typeof(T))
    { }
}
