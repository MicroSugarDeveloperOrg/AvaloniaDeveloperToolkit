using System;

namespace ReactiveUI;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
internal class RxPropertyAttribute : Attribute
{
}
