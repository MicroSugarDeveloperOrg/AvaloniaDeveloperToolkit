using SourceGeneratorToolkit.Builders;
using static Prism.SourceGenerators.Helpers.CodeHelpers;

namespace Prism.SourceGenerators.Builder;

internal static class CodeBuilderExtensions
{
    public static CodeBuilder AppendUsePropertySystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace("System");
        bRet = builder.AppendUseNameSpace(__PrismMvvmHeader__);
        return builder;
    }

    public static CodeBuilder AppendUseCommandSystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace("System");
        bRet = builder.AppendUseNameSpace("System.Windows.Input");
        bRet = builder.AppendUseNameSpace(__PrismCommandsHeader__);
        return builder;
    }

    public static CodeBuilder AppendUseIocSystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace("System");
        bRet = builder.AppendUseNameSpace(__PrismIocHeader__);
        return builder;
    }

}
