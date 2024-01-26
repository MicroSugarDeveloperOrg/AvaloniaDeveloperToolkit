namespace Prism.SourceGenerators.Builder;
internal static class CodeBuilderExtensions
{
    public static CodeBuilder AppendUseNameSpace(this CodeBuilder builder, string useNameSpace)
    {
        var bRet = builder.AppendUseNameSpace(useNameSpace);
        return builder;
    }

    public static CodeBuilder AppendUseSystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace("System");
        bRet = builder.AppendUseNameSpace("Prism.Commands");
        bRet = builder.AppendUseNameSpace("Prism.Mvvm");
        return builder;
    }

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

    public static CodeBuilder AppendCommand(this CodeBuilder builder, string argumentType, string methodName, string? canMethodName)
    {
        var bRet = builder.AppendCommand(argumentType, methodName, canMethodName);
        return builder;
    }
}
