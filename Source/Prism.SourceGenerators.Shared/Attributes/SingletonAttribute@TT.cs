namespace Prism.SourceGenerators.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class SingletonAttribute<TFrom,TTo> : SingletonAttribute where TFrom : TTo
{
    public SingletonAttribute() 
        : base(typeof(TFrom), typeof(TTo))
    { }
}
