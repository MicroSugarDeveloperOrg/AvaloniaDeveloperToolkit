﻿using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal sealed class ScopedAttribute<T> : ScopedAttribute
{
    public ScopedAttribute()
        : base(typeof(T))
    { }
 
}
