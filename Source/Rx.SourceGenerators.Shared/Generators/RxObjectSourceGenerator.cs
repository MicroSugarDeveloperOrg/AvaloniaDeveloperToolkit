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

            if (classSymbol.IsBaseOf(__RxObjectFull__))
                continue;

            if (classSymbol.BaseType?.ToDisplayString() != __object__)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateDuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError<RxObjectSourceGenerator>(__RxObject__),
                                         classSymbol.Locations.FirstOrDefault(),
                                         classSymbol.BaseType?.Name));
            }

            using CodeBuilder builder = CodeBuilder.CreateBuilder(classSymbol.ContainingNamespace.ToDisplayString(), classSymbol.Name, classSymbol.IsAbstract, default!);
            builder.AppendUsePropertySystemNameSpace();
            builder.AppendBaseType(__RxObject__);
            context.AddSource($"{classSymbol.Name}_{__RxObject__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }

        map.Clear();
        syntaxContextReceiver.Clear();
    }
}
