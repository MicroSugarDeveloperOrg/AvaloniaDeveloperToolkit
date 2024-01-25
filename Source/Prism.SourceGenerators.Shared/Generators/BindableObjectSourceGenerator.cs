using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Prism.SourceGenerators.Diagnostics;
using Prism.SourceGenerators.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class BindableObjectSourceGenerator : ISourceGenerator
{
    const string __bindableObjectAttributeEmbeddedResourceName__ = "BindableObjectAttribute";
    const string __bindableObjectEmbeddedResourceName__ = "BindableObject";

    public const string __bindableObjectAttribute__ = $"Prism.Mvvm.{__bindableObjectAttributeEmbeddedResourceName__}";
    public const string __bindableObject__ = $"Prism.Mvvm.{__bindableObjectEmbeddedResourceName__}";


    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context =>
        {
            context.CreateSourceCodeFromEmbeddedResource(__bindableObjectAttributeEmbeddedResourceName__);
            context.CreateSourceCodeFromEmbeddedResource(__bindableObjectEmbeddedResourceName__);
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
            INamedTypeSymbol namedTypeSymbol = mapClass.Key;
            ClassDeclarationSyntax classDeclarationSyntax = mapClass.Value;

            var baseType = namedTypeSymbol.BaseType;
            bool isBaseType = false;
            if (baseType is not null)
            {
                if (baseType.ToDisplayString() != __bindableObject__ && baseType.ToDisplayString() != "object")
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.DuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError,
                                             namedTypeSymbol.Locations.FirstOrDefault(),
                                             namedTypeSymbol));
                }
                else
                    isBaseType = true;
            }

            var classCode = CreateClassCode(namedTypeSymbol, isBaseType);
            if (string.IsNullOrWhiteSpace(classCode))
                continue;

            context.AddSource($"{namedTypeSymbol.Name}_bindableobject.g.cs", SourceText.From(classCode!, Encoding.UTF8));
        }

        syntaxContextReceiver.Clear();
    }

    class SyntaxContextReceiver : ISyntaxContextReceiver
    {
        Dictionary<INamedTypeSymbol, ClassDeclarationSyntax> _mapClasses = [];

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

                    if (type.ToDisplayString() != __bindableObjectAttribute__)
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

                _mapClasses[namedTypeSymbol] = classDeclaration;
            }
        }

        public Dictionary<INamedTypeSymbol, ClassDeclarationSyntax> GetClasses() => _mapClasses;

        public bool Clear()
        {
            _mapClasses.Clear();
            _mapClasses = null!;
            return true;
        }

    }

    string? CreateClassCode(INamedTypeSymbol classSymbol, bool isBaseType)
    {
        if (classSymbol is null)
            return default;
    
        if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return default;

        string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
        string classDescription = isBaseType ? $"partial class {classSymbol.Name} " : $"partial class {classSymbol.Name} : {__bindableObject__} ";

        string code =  $"""
           namespace {namespaceName};
           {classDescription}
           {'{'}
           {'}'}        
        """;
        return code;
    }
}
