namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal class SingletonAttribute<T> : SingletonAttribute
{
    public SingletonAttribute() 
        : base(typeof(T))
    { }
}
