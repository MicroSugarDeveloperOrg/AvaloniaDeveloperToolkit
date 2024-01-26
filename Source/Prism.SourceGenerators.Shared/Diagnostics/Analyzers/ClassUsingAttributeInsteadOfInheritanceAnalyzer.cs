using Prism.SourceGenerators.Extensions;
using Prism.SourceGenerators.Generators;
using System.Collections.Generic;
using System.Linq;
using static Prism.SourceGenerators.Helpers.CodeHelpers;

namespace Prism.SourceGenerators.Diagnostics.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ClassUsingAttributeInsteadOfInheritanceAnalyzer : DiagnosticAnalyzer
{
    internal const string TypeNameKey = "TypeName";

    internal const string AttributeTypeNameKey = "AttributeTypeName";

    private static readonly ImmutableDictionary<string, string> GeneratorAttributeNamesToFullyQualifiedNamesMap = ImmutableDictionary.CreateRange(new[]
    {
        new KeyValuePair<string, string>("BindableObjectAttribute", "Prism.Mvvm.BindableObjectAttribute"), 
    });

    private static readonly ImmutableDictionary<string, DiagnosticDescriptor> GeneratorAttributeNamesToDiagnosticsMap = ImmutableDictionary.CreateRange(new[]
    {
        new KeyValuePair<string, DiagnosticDescriptor>("BindableObjectAttribute", DiagnosticDescriptors.InheritFromBindableObjectInsteadOfUsingBindableObjectAttributeWarning),
    });

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.InheritFromBindableObjectInsteadOfUsingBindableObjectAttributeWarning);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(static context =>
        {
            if (!context.Compilation.TryBuildNamedTypeSymbolMap(GeneratorAttributeNamesToFullyQualifiedNamesMap, out ImmutableDictionary<string, INamedTypeSymbol>? typeSymbols))
                return;

            context.RegisterSymbolAction(context =>
            {
                if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Class, IsRecord: false, IsStatic: false, IsImplicitlyDeclared: false, BaseType.SpecialType: SpecialType.System_Object } classSymbol)
                    return;

                var baseType = classSymbol.BaseType;
                if (baseType is not null)
                {
                    foreach (AttributeData attribute in context.Symbol.GetAttributes())
                    {
                        if (attribute.AttributeClass is { Name: string attributeName } attributeClass &&
                            typeSymbols.TryGetValue(attributeName, out INamedTypeSymbol? attributeSymbol) &&
                            SymbolEqualityComparer.Default.Equals(attributeClass, attributeSymbol))
                        {
                            if (baseType.ToDisplayString() != __BindableObject__ && baseType.ToDisplayString() != "object")
                            {
                                context.ReportDiagnostic(Diagnostic.Create(
                                    DiagnosticDescriptors.DuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError,
                                        context.Symbol.Locations.FirstOrDefault(),
                                         ImmutableDictionary.Create<string, string?>()
                                            .Add(TypeNameKey, classSymbol.Name)
                                            .Add(AttributeTypeNameKey, attributeName),
                                            context.Symbol));
                            }
                        }
                    }
                }
                else
                {
                    foreach (AttributeData attribute in context.Symbol.GetAttributes())
                    {
                        if (attribute.AttributeClass is { Name: string attributeName } attributeClass &&
                            typeSymbols.TryGetValue(attributeName, out INamedTypeSymbol? attributeSymbol) &&
                            SymbolEqualityComparer.Default.Equals(attributeClass, attributeSymbol))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                GeneratorAttributeNamesToDiagnosticsMap[attributeClass.Name],
                                context.Symbol.Locations.FirstOrDefault(),
                                ImmutableDictionary.Create<string, string?>()
                                    .Add(TypeNameKey, classSymbol.Name)
                                    .Add(AttributeTypeNameKey, attributeName),
                                context.Symbol));
                        }
                    }
                }
            }, SymbolKind.NamedType);
        });
    }
}
