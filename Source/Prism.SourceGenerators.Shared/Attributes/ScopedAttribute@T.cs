namespace Prism.SourceGenerators.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class ScopedAttribute<T> : ScopedAttribute
{
    public ScopedAttribute()
        : base(typeof(T))
    { }
 
}
