namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal class NavigationAttribute<T> : InjectAttribute
{
    public NavigationAttribute() 
        : base(typeof(T))
    {
    }
}
