using SourceGeneratorToolkit.Builders;

namespace SourceGeneratorToolkit.Extensions;

internal static class CodeBuildXExtensions  
{
    public static CodeBuilderX AppendMethodX(this CodeBuilderX builder, string methodName, string? argument, string? returnType)
    {
        var bRet = builder.AppendMethod(methodName, argument, returnType);
        return builder;
    }

    public static CodeBuilderX AppendMethodElementX(this CodeBuilderX builder, string methodName, MethodElement element)
    {
        var bRet = builder.AppendMethodElement(methodName, element);
        return builder;
    }
}
