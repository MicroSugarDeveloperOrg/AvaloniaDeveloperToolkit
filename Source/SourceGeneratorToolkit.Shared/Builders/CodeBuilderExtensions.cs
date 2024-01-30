namespace SourceGeneratorToolkit.Builders;

internal static class CodeBuilderExtensions
{
    public static CodeBuilder AppendBaseType(this CodeBuilder builder, string baseType)
    {
        var bRet = builder.AppendBaseType(baseType);
        return builder;
    }

    public static CodeBuilder AppendInterface(this CodeBuilder builder, string interfaceName)
    {
        var bRet = builder.AppendInterface(interfaceName);
        return builder;
    }

    public static CodeBuilder AppendProperty(this CodeBuilder builder, string type, string fieldName, string propertyName)
    {
        var bRet = builder.AppendProperty(type, fieldName, propertyName);
        return builder;
    }

    public static CodeBuilder AppendCommand(this CodeBuilder builder, string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        var bRet = builder.AppendCommand(argumentType, returnType, methodName, canMethodName);
        return builder;
    }
}
