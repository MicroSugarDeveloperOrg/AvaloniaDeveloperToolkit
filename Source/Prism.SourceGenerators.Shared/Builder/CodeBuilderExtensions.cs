using SourceGeneratorToolkit.Builders;

namespace Prism.SourceGenerators.Builder;

internal static class CodeBuilderExtensions
{
    public static CodeBuilder AppendUsePropertySystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace("System");
        bRet = builder.AppendUseNameSpace("Prism.Mvvm");
        return builder;
    }

    public static CodeBuilder AppendUseCommandSystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace("System");
        bRet = builder.AppendUseNameSpace("System.Windows.Input");
        bRet = builder.AppendUseNameSpace("Prism.Commands");
        return builder;
    }
     
}
