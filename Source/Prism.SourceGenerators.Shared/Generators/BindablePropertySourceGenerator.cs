using Prism.SourceGenerators.Builder;
using Prism.SourceGenerators.Diagnostics;
using Prism.SourceGenerators.Extensions;
using static Prism.SourceGenerators.Helpers.CodeHelpers;

namespace Prism.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class BindablePropertySourceGenerator : ISourceGenerator
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(__BindablePropertyAttributeEmbeddedResourceName__));
        context.RegisterForSyntaxNotifications(() => new SyntaxContextReceiver());
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxContextReceiver;
        if (receiver is not SyntaxContextReceiver syntaxContextReceiver)
            return;

        var map = syntaxContextReceiver.GetFields();
        if (map.Count <= 0)
            return;

        foreach (var mapField in map)
        {
            INamedTypeSymbol classSymbol = mapField.Key;

            using CodeBuilder builder = CodeBuilder.CreateBuilder(classSymbol.Name, classSymbol.ContainingNamespace.ToDisplayString());
            builder.AppendUsePropertySystemNameSpace();

            ImmutableArray<IFieldSymbol> fieldSymbols = mapField.Value;

            foreach (var fieldSymbol in fieldSymbols)
            {
                var propertyName = fieldSymbol.CreateGeneratedPropertyName();
                if (propertyName == fieldSymbol.Name)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.BindablePropertyNameCollisionError,
                                            fieldSymbol.Locations.FirstOrDefault(),
                                            classSymbol));
                    continue;
                }

                builder.AppendProperty(fieldSymbol.Type.ToDisplayString(), fieldSymbol.Name, propertyName);
            }

            context.AddSource($"{classSymbol.Name}_{__BindableProperty__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }

        map.Clear();
        syntaxContextReceiver.Clear();
    }

    class SyntaxContextReceiver : ISyntaxContextReceiver
    {
        Dictionary<INamedTypeSymbol, List<IFieldSymbol>> _mapFields = [];

        void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax && fieldDeclarationSyntax.AttributeLists.Count > 0)
            {
                foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
                {
                    var symbol = context.SemanticModel.GetDeclaredSymbol(variable);
                    if (symbol is not IFieldSymbol fieldSymbol)
                        continue;

                    if (fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass?.ToDisplayString() == __BindablePropertyAttribute__))
                    {
                        //Debugger.Launch();

                        var type = fieldSymbol.ContainingType;
                        var bRet = _mapFields.TryGetValue(type, out var value);
                        if (value is null)
                        {
                            value = new List<IFieldSymbol>();
                            _mapFields.Add(type, value);
                        }

                        value.Add(fieldSymbol);
                    }
                }
            }
        }

        public ImmutableDictionary<INamedTypeSymbol, ImmutableArray<IFieldSymbol>> GetFields()
        {
            Dictionary<INamedTypeSymbol, ImmutableArray<IFieldSymbol>> map = [];

            foreach (var item in _mapFields)
                map.Add(item.Key, item.Value.ToImmutableArray());

            var returnMap = map.ToImmutableDictionary(default);

            return returnMap;
        }

        public bool Clear()
        {
            foreach (var item in _mapFields)
                item.Value.Clear();

            _mapFields.Clear();
            _mapFields = null!;
            return true;
        }

    }



}
