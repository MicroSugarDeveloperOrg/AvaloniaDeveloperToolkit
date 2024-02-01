using System;

namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal sealed class NavigationAttribute<TView,TViewModel> : InjectAttribute
{
    public NavigationAttribute() 
        : base(typeof(TView), typeof(TViewModel))
    { }
}
