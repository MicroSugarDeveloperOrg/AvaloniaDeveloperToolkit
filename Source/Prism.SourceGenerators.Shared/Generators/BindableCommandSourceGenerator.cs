using Prism.Commands;
using Prism.SourceGenerators.Builder;
using Prism.SourceGenerators.Diagnostics;
using Prism.SourceGenerators.Extensions;
using static Prism.SourceGenerators.Helpers.CodeHelpers;

namespace Prism.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class BindableCommandSourceGenerator : ISourceGenerator
{
    const string __bindableCommandAttributeEmbeddedResourceName__ = "BindableCommandAttribute";

    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(__bindableCommandAttributeEmbeddedResourceName__));
        context.RegisterForSyntaxNotifications(() => new SyntaxContextReceiver());
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxContextReceiver;
        if (receiver is not SyntaxContextReceiver syntaxContextReceiver)
            return;

        var map = syntaxContextReceiver.GetMethods();
        if (map.Count <= 0)
            return;

        //Debugger.Launch();
        foreach (var mapMethod in map)
        {
            INamedTypeSymbol classSymbol = mapMethod.Key;
            using CodeBuilder builder = CodeBuilder.CreateBuilder(classSymbol.Name, classSymbol.ContainingNamespace.ToDisplayString());
            builder.AppendUseCommandSystemNameSpace();

            ImmutableArray<IMethodSymbol> methodSymbols = mapMethod.Value;

            foreach (var methodSymbol in methodSymbols)
            {
                if (methodSymbol is null)
                    continue;

                if (!methodSymbol.ReturnsVoid || methodSymbol.Parameters.Length > 1)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.InvalidBindableCommandMethodSignatureError,
                                            methodSymbol.Locations.FirstOrDefault(),
                                            classSymbol));
                    continue;
                }

                string? parameterType = default;
                var parameterSymbol = methodSymbol.Parameters.FirstOrDefault();
                if (parameterSymbol is not null)
                    parameterType = parameterSymbol.Type.GetFullyQualifiedName();

                string? canExcMethod = default;
                var attributeData = methodSymbol.GetAttributes().FirstOrDefault(ad => ad.AttributeClass?.ToDisplayString() == __BindableCommandFullAttribute__);
                if (attributeData is not null)
                {
                    //get can execute name from  NamedArgument
                    attributeData.TryGetNamedArgument<string>(nameof(BindableCommandAttribute.CanExecuteString), out var value);
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        //get can execute name from constructor 
                        var arguments = attributeData.ConstructorArguments.FirstOrDefault();
                        value = arguments.Value?.ToString();
                    }
                    canExcMethod = value;
                    if (value is not null)
                    {
                        //check  canexcmethod Signature 
                        var canMethodSymbol = classSymbol.GetMembers(value).FirstOrDefault() as IMethodSymbol;
                        if (canMethodSymbol is not null)
                        {
                            if (canMethodSymbol.ReturnType.SpecialType != SpecialType.System_Boolean
                                && canMethodSymbol.Parameters.Length != methodSymbol.Parameters.Length)
                            {
                                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.InvalidCanExecuteMemberNameError,
                                           methodSymbol.Locations.FirstOrDefault(),
                                           classSymbol));
                                continue;
                            }

                            var canMethodParameterSymbol = canMethodSymbol.Parameters.FirstOrDefault();
                            if (canMethodParameterSymbol is not null && canMethodParameterSymbol.Type.GetFullyQualifiedName() != parameterType)
                            {
                                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.InvalidCanExecuteMemberNameError,
                                         methodSymbol.Locations.FirstOrDefault(),
                                         classSymbol));
                                continue;
                            }
                        }
                    }
                }

                builder.AppendCommand(parameterType, methodSymbol.GetGeneratedMethodName(), canExcMethod);
            }

            context.AddSource($"{classSymbol.Name}_{__BindableCommand__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }
    }

    class SyntaxContextReceiver : ISyntaxContextReceiver
    {
        Dictionary<INamedTypeSymbol, List<IMethodSymbol>> _mapMethods = [];

        void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is not MethodDeclarationSyntax methodDeclarationSyntax || methodDeclarationSyntax.AttributeLists.Count <= 0)
                return;

            var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
            if (methodSymbol is null)
                return;

            //Debugger.Launch();
            if (!methodSymbol.GetAttributes().Any(ad => ad.AttributeClass?.ToDisplayString() == __BindableCommandFullAttribute__))
                return;

            var type = methodSymbol.ContainingType;
            _mapMethods.TryGetValue(type, out var value);
            if (value is null)
            {
                value = new List<IMethodSymbol>();
                _mapMethods.Add(type, value);
            }

            value.Add(methodSymbol);
        }

        public ImmutableDictionary<INamedTypeSymbol, ImmutableArray<IMethodSymbol>> GetMethods()
        {
            Dictionary<INamedTypeSymbol, ImmutableArray<IMethodSymbol>> map = [];

            foreach (var item in _mapMethods)
                map.Add(item.Key, item.Value.ToImmutableArray());

            var returnMap = map.ToImmutableDictionary(default);
            map.Clear();
            return returnMap;
        }

        public bool Clear()
        {
            foreach (var item in _mapMethods)
                item.Value.Clear();

            _mapMethods.Clear();
            _mapMethods = null!;
            return true;
        }
    }
}
