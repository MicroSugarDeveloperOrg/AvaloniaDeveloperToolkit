namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal class ManyScopedAttribute<T> : ManyInjectAttribute
{
    public ManyScopedAttribute(params Type[] serviceTypes)
       : base(typeof(T), serviceTypes)
    { }
}
