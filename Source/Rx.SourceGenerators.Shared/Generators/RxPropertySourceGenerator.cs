using Rx.SourceGenerator.Builder;
using SourceGeneratorToolkit.Builders;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using SourceGeneratorToolkit.SyntaxContexts;
using static Rx.SourceGenerators.Helpers.CodeHelpers;
using static SourceGeneratorToolkit.Helpers.CommonHelpers;

namespace Rx.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class RxPropertySourceGenerator : ISourceGenerator, ICodeProvider
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(__RxPropertyAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__));
        context.RegisterForSyntaxNotifications(() => new PropertySyntaxContextReceiver(__RxPropertyFullAttribute__));
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxContextReceiver;
        if (receiver is not PropertySyntaxContextReceiver syntaxContextReceiver)
            return;

        var map = syntaxContextReceiver.GetFields();

        foreach (var mapSymbol in map)
        {
            var classSymbol = mapSymbol.Key;
            var fieldSymbols = mapSymbol.Value;

            if (classSymbol is null || fieldSymbols.Length <= 0)
                continue;

            using CodeBuilder builder = CodeBuilder.CreateBuilder(classSymbol.ContainingNamespace.ToDisplayString(), classSymbol.Name, this);
            builder.AppendUsePropertySystemNameSpace();

            foreach (var fieldSymbol in fieldSymbols)
            {
                var propertyName = fieldSymbol.CreateGeneratedPropertyName();
                if (propertyName == fieldSymbol.Name)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateBindablePropertyNameCollisionError<RxPropertySourceGenerator>(__RxProperty__),
                                            fieldSymbol.Locations.FirstOrDefault(),
                                            classSymbol.Name,
                                            fieldSymbol.Name));
                    continue;
                }

                builder.AppendProperty(fieldSymbol.Type.ToDisplayString(), fieldSymbol.Name, propertyName);
            }

            context.AddSource($"{classSymbol.Name}_{__RxProperty__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }

    }

    string ICodeProvider.GetRaisePropertyString(string fieldName, string propertyName)
    {
        return $"this.RaiseAndSetIfChanged(ref {fieldName}, value)";
    }

    string ICodeProvider.CreateCommandString(string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        throw new NotImplementedException();
    }
}
