using Rx.SourceGenerator.Builder;
using SourceGeneratorToolkit.Builders;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using SourceGeneratorToolkit.SyntaxContexts;
using static Rx.SourceGenerators.Helpers.CodeHelpers;
using static SourceGeneratorToolkit.Helpers.CommonHelpers;

namespace Rx.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class RxObjectSourceGenerator : ISourceGenerator
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(__RxObjectAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__));
        context.RegisterForSyntaxNotifications(() => new ObjectSyntaxContextReceiver(__RxObjectFullAttribute__));
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxContextReceiver;
        if (receiver is not ObjectSyntaxContextReceiver syntaxContextReceiver)
            return;

        var map = syntaxContextReceiver.GetClasses();

        foreach (var classSymbol in map)
        {
            if (classSymbol is null)
                continue;

            //Debugger.Launch();
            using CodeBuilder builder = CodeBuilder.CreateBuilder(classSymbol.ContainingNamespace.ToDisplayString(), classSymbol.Name, default!);
            var baseType = classSymbol.BaseType;

            if (baseType is not null)
            {
                if (baseType.ToDisplayString() != __RxObjectFull__)
                {
                    if (baseType.ToDisplayString() != __object__)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateDuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError<RxObjectSourceGenerator>(__RxObject__),
                                                 classSymbol.Locations.FirstOrDefault(),
                                                 baseType.Name));
                    }
                }
                else
                    continue;
            }

            builder.AppendUsePropertySystemNameSpace();
            builder.AppendBaseType(__RxObject__);
            context.AddSource($"{classSymbol.Name}_{__RxObjectAttributeEmbeddedResourceName__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }

        map.Clear();
        syntaxContextReceiver.Clear();
    }
}
