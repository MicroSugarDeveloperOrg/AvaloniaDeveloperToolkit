using SourceGeneratorToolkit.Builders;

namespace SourceGeneratorToolkit.Extensions;

internal static class CodeBuilderExtensions
{
    public static CodeBuilder AppendUseNameSpaceX(this CodeBuilder builder, string useNameSpace)
    {
        var bRet = builder.AppendUseNameSpace(useNameSpace);
        return builder;
    }

    public static CodeBuilder AppendBaseTypeX(this CodeBuilder builder, string baseType)
    {
        var bRet = builder.AppendBaseType(baseType);
        return builder;
    }

    public static CodeBuilder AppendInterfaceX(this CodeBuilder builder, string interfaceName)
    {
        var bRet = builder.AppendInterface(interfaceName);
        return builder;
    }

    public static CodeBuilder AppendPropertyX(this CodeBuilder builder, string type, string fieldName, string propertyName)
    {
        var bRet = builder.AppendProperty(type, fieldName, propertyName);
        return builder;
    }

    public static CodeBuilder AppendCommandX(this CodeBuilder builder, string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        var bRet = builder.AppendCommand(argumentType, returnType, methodName, canMethodName);
        return builder;
    }
}
