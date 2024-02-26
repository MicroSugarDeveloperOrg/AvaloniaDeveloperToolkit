using System;

namespace Prism.Ioc;

#nullable enable

internal class InjectAttribute : Attribute
{
    public InjectAttribute(Type to)
    {
        To = to;
    }

    public InjectAttribute(Type from, Type to)
        : this(from)
    {
        From = from;
    }

    public Type? From { get; init; }
    public Type To { get; init; }
    public string? Token { get; init; }
}

#nullable disable