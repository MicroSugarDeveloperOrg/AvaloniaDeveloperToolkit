namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal sealed class ManyTransientAttribute<T> : ManyInjectAttribute
{
    public ManyTransientAttribute(params Type[] serviceTypes)
       : base(typeof(T), serviceTypes)
    { }
}
