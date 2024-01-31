using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class NavigationAttribute<TView,TViewModel> : InjectAttribute
{
    public NavigationAttribute() 
        : base(typeof(TView), typeof(TViewModel))
    { }
}
