using System;

namespace Prism.Ioc;

#nullable enable

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal class SingletonAttribute : InjectAttribute
{
    public SingletonAttribute(Type to) 
        : base(to)
    { }

    public SingletonAttribute(Type from, Type to) 
        : base(from, to)
    { }
}

#nullable disable
