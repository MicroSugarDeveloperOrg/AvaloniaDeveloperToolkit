using System;

namespace Prism.Commands;

#nullable disable

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
internal sealed class BindableCommandAttribute : Attribute
{
    public BindableCommandAttribute()
    {

    }

    public BindableCommandAttribute(string canExecuteString)
    {
        CanExecuteString = canExecuteString;
    }

    public string CanExecuteString { get; set; }
}

#nullable enable