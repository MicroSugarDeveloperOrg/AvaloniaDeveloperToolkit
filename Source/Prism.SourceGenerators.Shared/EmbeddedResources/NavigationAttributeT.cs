﻿using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class NavigationAttribute<T> : InjectAttribute
{
    public NavigationAttribute() 
        : base(typeof(T))
    {
    }
}