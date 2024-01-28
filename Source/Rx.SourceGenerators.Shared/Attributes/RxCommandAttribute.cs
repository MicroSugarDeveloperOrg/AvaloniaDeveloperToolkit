namespace Rx.SourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
internal class RxCommandAttribute : Attribute
{
    public RxCommandAttribute()
    {

    }

    public RxCommandAttribute(string? canExecuteString)
    {
        CanExecuteString = canExecuteString;
    }

    public string? CanExecuteString { get; set; }
}
