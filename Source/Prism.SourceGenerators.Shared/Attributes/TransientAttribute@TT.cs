namespace Prism.SourceGenerators.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class TransientAttribute<TFrom, TTo> : TransientAttribute
{
    public TransientAttribute()
        : base(typeof(TFrom), typeof(TTo))
    { }
}
