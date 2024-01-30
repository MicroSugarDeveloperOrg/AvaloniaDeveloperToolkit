﻿using Prism.SourceGenerators.Builder;
using SourceGeneratorToolkit.Builders;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using SourceGeneratorToolkit.SyntaxContexts;
using static Prism.SourceGenerators.Helpers.CodeHelpers;
using static SourceGeneratorToolkit.Helpers.CommonHelpers;

namespace Prism.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class BindableObjectSourceGenerator : ISourceGenerator
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {         
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(__BindableObjectAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__));
        context.RegisterForSyntaxNotifications(() => new ObjectSyntaxContextReceiver(__BindableObjectFullAttribute__));
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

            using CodeBuilder builder = CodeBuilder.CreateBuilder(classSymbol.ContainingNamespace.ToDisplayString(), classSymbol.Name, default!);
            var baseType = classSymbol.BaseType;

            if (baseType is not null)
            {
                if (baseType.ToDisplayString() != __BindableFullObject__)
                {
                    if (baseType.ToDisplayString() != __object__)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateDuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError<BindableObjectSourceGenerator>(__BindableObject__),
                                                 classSymbol.Locations.FirstOrDefault(),
                                                 classSymbol.Name));
                    }
                }
                else
                    continue;
            }

            builder.AppendUsePropertySystemNameSpace();
            builder.AppendBaseType(__BindableObject__);

            context.AddSource($"{classSymbol.Name}_{__BindableObject__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }

        map.Clear();
        syntaxContextReceiver.Clear();
    }
}
