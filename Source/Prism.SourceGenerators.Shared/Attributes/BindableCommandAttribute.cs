using System;

namespace Prism.Commands;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
internal sealed class BindableCommandAttribute : Attribute
{
}
