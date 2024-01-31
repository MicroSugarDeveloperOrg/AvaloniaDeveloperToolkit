namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class ManySingletonAttribute<T> : ManyInjectAttribute
{
    public ManySingletonAttribute(params Type[] serviceTypes) 
        : base(typeof(T), serviceTypes)
    { }
}
