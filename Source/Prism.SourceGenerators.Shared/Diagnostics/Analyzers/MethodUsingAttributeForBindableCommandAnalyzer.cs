using Prism.SourceGenerators.Generators;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using static Prism.SourceGenerators.Helpers.CodeHelpers;

namespace Prism.SourceGenerators.Diagnostics.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class MethodUsingAttributeForBindableCommandAnalyzer : DiagnosticAnalyzer
{
    internal const string MethodNameKey = "MethodName";

    internal const string ArgumentCountNameKey = "TypeArguments";

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.CreateInvalidBindableCommandMethodSignatureError<BindableCommandSourceGenerator>(__BindableCommand__));

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(static context =>
        {
            if (context.Compilation.GetTypeByMetadataName(__BindableCommandFullAttribute__) is not INamedTypeSymbol bindableCommandSymbol)
                return;

            context.RegisterSymbolAction(context =>
            {
                if (context.Symbol is not IMethodSymbol methodSymbol)
                    return;

                if (!methodSymbol.HasAttributeWithType(bindableCommandSymbol))
                    return;

                if (!methodSymbol.ReturnsVoid || methodSymbol.Parameters.Length > 1)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidBindableCommandMethodSignatureError<BindableCommandSourceGenerator>(__BindableCommand__),
                                             context.Symbol.Locations.FirstOrDefault(),
                                             ImmutableDictionary.Create<string, string?>()
                                                         .Add(MethodNameKey, methodSymbol.Name)
                                                         .Add(ArgumentCountNameKey, methodSymbol.TypeArguments.Length.ToString()),
                                             context.Symbol));
                }

            }, SymbolKind.Method);
        });
    }
}
