namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class ManyTransientAttribute<T> : ManyInjectAttribute
{
    public ManyTransientAttribute(params Type[] serviceTypes)
       : base(typeof(T), serviceTypes)
    { }
}
