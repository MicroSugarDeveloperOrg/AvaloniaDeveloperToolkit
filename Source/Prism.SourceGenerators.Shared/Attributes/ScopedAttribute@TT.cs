namespace Prism.SourceGenerators.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class ScopedAttribute<TFrom, TTo> : ScopedAttribute
{
    public ScopedAttribute()
        : base(typeof(TFrom), typeof(TTo))
    { }
}
