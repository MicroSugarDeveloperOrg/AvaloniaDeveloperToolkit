namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal sealed class ManySingletonAttribute<T> : ManyInjectAttribute
{
    public ManySingletonAttribute(params Type[] serviceTypes) 
        : base(typeof(T), serviceTypes)
    { }
}
