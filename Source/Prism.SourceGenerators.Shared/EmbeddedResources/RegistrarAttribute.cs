namespace Prism.Ioc;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal sealed class RegistrarAttribute(string registerMethodString) : Attribute
{
    public string RegisterMethodString { get; } = registerMethodString;
}
