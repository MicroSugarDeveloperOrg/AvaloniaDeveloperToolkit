using SourceGeneratorToolkit.Builders;

namespace Rx.SourceGenerator.Builder;

internal static class CodeBuilderExtensions
{
    public static CodeBuilder AppendUsePropertySystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace("System");
        bRet = builder.AppendUseNameSpace("ReactiveUI");
        return builder;
    }

    public static CodeBuilder AppendUseCommandSystemNameSpace(this CodeBuilder builder)
    {
        var bRet = builder.AppendUseNameSpace("System");
        bRet = builder.AppendUseNameSpace("System.Windows.Input");
        bRet = builder.AppendUseNameSpace("ReactiveUI");
        bRet = builder.AppendUseNameSpace("System.Reactive.Linq");
        return builder;
    }
}
