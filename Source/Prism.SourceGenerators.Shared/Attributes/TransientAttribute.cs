﻿namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class TransientAttribute : InjectAttribute
{
    public TransientAttribute(Type to)
        : base(to)
    { }

    public TransientAttribute(Type from, Type to)
        : base(from, to)
    { }
}