using Microsoft.CodeAnalysis.Text;
using Prism.SourceGenerators.Diagnostics;
using Prism.SourceGenerators.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Prism.SourceGenerators.Helpers.CodeHelpers;

namespace Prism.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class BindableObjectSourceGenerator : ISourceGenerator
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context =>
        {
            context.CreateSourceCodeFromEmbeddedResource(__BindableObjectAttributeEmbeddedResourceName__);
            context.CreateSourceCodeFromEmbeddedResource(__BindableObjectEmbeddedResourceName__);
        });

        context.RegisterForSyntaxNotifications(() => new SyntaxContextReceiver());
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxContextReceiver;
        if (receiver is not SyntaxContextReceiver syntaxContextReceiver)
            return;

        var map = syntaxContextReceiver.GetClasses();
        foreach (var mapClass in map)
        {
            INamedTypeSymbol namedTypeSymbol = mapClass;

            var baseType = namedTypeSymbol.BaseType;
            bool isBaseType = false;

            if (baseType is not null)
            {
                if (baseType.ToDisplayString() != __BindableObject__)
                {
                    if (baseType.ToDisplayString() != __object__)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.DuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError,
                                            namedTypeSymbol.Locations.FirstOrDefault(),
                                            namedTypeSymbol));
                    }
                }
                else
                    isBaseType = true;
            }

           
            var classCode = BuildClassCode(namedTypeSymbol, isBaseType);
            if (string.IsNullOrWhiteSpace(classCode))
                continue;
         
            context.AddSource($"{namedTypeSymbol.Name}_{__BindableObjectEmbeddedResourceName__}.{__GeneratorCSharpFileExtension__}", SourceText.From(classCode!, Encoding.UTF8));
        }

        map.Clear();
        syntaxContextReceiver.Clear();
    }

    class SyntaxContextReceiver : ISyntaxContextReceiver
    {
        List<INamedTypeSymbol> _mapClasses = [];

        void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is not ClassDeclarationSyntax classDeclaration)
                return;

            if (classDeclaration.AttributeLists.Count <= 0)
                return;

            bool isFlag = false;
            foreach (var item in classDeclaration.AttributeLists)
            {
                if (item is null)
                    continue;

                if (item.Attributes.Count <= 0)
                    continue;

                foreach (var attributeSyntax in item.Attributes)
                {
                    var type = context.SemanticModel.GetTypeInfo(attributeSyntax).Type;
                    if (type is null)
                        continue;

                    if (type.ToDisplayString() != __BindableObjectAttribute__)
                        continue;

                    isFlag = true;
                    break;
                }

                if (isFlag)
                    break;
            }

            if (isFlag)
            {
                var namedTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
                if (namedTypeSymbol is null)
                    return;

                //Debugger.Launch();
                _mapClasses.Add(namedTypeSymbol);
            }
        }

        public ImmutableArray<INamedTypeSymbol> GetClasses() => _mapClasses.ToImmutableArray();

        public bool Clear()
        {
            _mapClasses.Clear();
            _mapClasses = null!;
            return true;
        }

    }

    string? BuildClassCode(INamedTypeSymbol classSymbol, bool isBaseType)
    {
        if (classSymbol is null)
            return default;

        if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return default;

        string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
        string classDescription = isBaseType ? $"partial class {classSymbol.Name} " : $"partial class {classSymbol.Name} : {__BindableObject__} ";

        string code = $"""
           namespace {namespaceName};
           {classDescription}
           {'{'}
           {'}'}        
        """;
        return code;
    }
}
