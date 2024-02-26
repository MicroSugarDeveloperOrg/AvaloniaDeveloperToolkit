using SourceGeneratorToolkit.Builders;

namespace Mvvm.SourceGenerators.Builder;

internal static class CodeBuilderExtensions
{
    public static CodeBuilder AppendUseSystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace(nameof(System));
        bRet = builder.AppendUseNameSpace($"{nameof(System)}.{nameof(System.ComponentModel)}");
        bRet = builder.AppendUseNameSpace("System.Runtime.CompilerServices");
        return builder;
    }

    public static CodeBuilder AppendUsePropertySystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace("System");
        return builder;
    }
}
