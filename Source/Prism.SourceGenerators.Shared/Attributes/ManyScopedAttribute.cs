namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class ManyScopedAttribute<T> : ManyInjectAttribute
{
    public ManyScopedAttribute(params Type[] serviceTypes)
       : base(typeof(T), serviceTypes)
    { }
}
