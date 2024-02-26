using Rx.SourceGenerator.Attributes;
using Rx.SourceGenerator.Builder;
using SourceGeneratorToolkit.Builders;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using SourceGeneratorToolkit.SyntaxContexts;
using static Rx.SourceGenerators.Helpers.CodeHelpers;
using static SourceGeneratorToolkit.Helpers.CommonHelpers;

namespace Rx.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class RxCommandSourceGenerator : ISourceGenerator, ICodeProvider
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(__RxCommandAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__));
        context.RegisterForSyntaxNotifications(() => new CommandSyntaxContextReceiver(__RxCommandFullAttribute__));
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxContextReceiver;
        if (receiver is not CommandSyntaxContextReceiver syntaxContextReceiver)
            return;

        var map = syntaxContextReceiver.GetMethods();
        if (map.Count <= 0)
            return;

        foreach (var mapMethod in map)
        {
            INamedTypeSymbol classSymbol = mapMethod.Key;
            using CodeBuilder builder = CodeBuilder.CreateBuilder(classSymbol.ContainingNamespace.ToDisplayString(), classSymbol.Name, classSymbol.IsAbstract, this);
            builder.AppendUseCommandSystemNameSpace();

            ImmutableArray<IMethodSymbol> methodSymbols = mapMethod.Value;

            foreach (var methodSymbol in methodSymbols)
            {
                if (methodSymbol is null)
                    continue;

                if (methodSymbol.Parameters.Length > 1)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidBindableCommandMethodSignatureError<RxCommandSourceGenerator>(__RxCommand__),
                                            methodSymbol.Locations.FirstOrDefault(),
                                            classSymbol.Name,
                                            methodSymbol.Name));
                    continue;
                }

                string? parameterType = default;
                var parameterSymbol = methodSymbol.Parameters.FirstOrDefault();
                if (parameterSymbol is not null)
                    parameterType = parameterSymbol.Type.GetFullyQualifiedName();

                string? canExcMethod = default;
                var attributeData = methodSymbol.GetAttributes().FirstOrDefault(ad => ad.AttributeClass?.ToDisplayString() == __RxCommandFullAttribute__);
                if (attributeData is not null)
                {
                    //get can execute name from  NamedArgument
                    attributeData.TryGetNamedArgument<string>(nameof(RxCommandAttribute.CanExecuteString), out var value);
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
                                || canMethodSymbol.Parameters.Length > 0)
                            {
                                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidCanExecuteMemberNameError<RxCommandSourceGenerator>(__RxCommand__),
                                                         methodSymbol.Locations.FirstOrDefault(),
                                                         $"{classSymbol.Name}.{canMethodSymbol.Name}",
                                                         canMethodSymbol.ReturnType.Name));
                                continue;
                            }
                        }
                    }
                }

                //Debugger.Launch();
                builder.AppendCommand(parameterType, methodSymbol.ReturnType.ToDisplayString(), methodSymbol.GetGeneratedMethodName(), canExcMethod);
            }

            context.AddSource($"{classSymbol.Name}_{__RxCommand__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }
    }

    string ICodeProvider.GetRaisePropertyString(string fieldName, string propertyName)
    {
        throw new NotImplementedException();
    }

    string ICodeProvider.CreateCommandString(string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        var argumentTypeString = string.IsNullOrWhiteSpace(argumentType) ? "Unit" : argumentType;
        var returnTypeString = string.IsNullOrWhiteSpace(returnType) ? "Unit" : returnType;
        string code;

        var lowMethodName = $"_{methodName.FirstCharToLow()}Command";

        if (string.IsNullOrWhiteSpace(canMethodName))
            code =
                $"""
                    ReactiveCommand<{argumentTypeString},{returnTypeString}>? {lowMethodName};
                    public ICommand {methodName}Command => {lowMethodName} ??= ReactiveCommand.Create<{argumentTypeString},{returnTypeString}>({methodName});
                """;
        else
            code =
                $"""
                    ReactiveCommand<{argumentTypeString},{returnTypeString}>? {lowMethodName};
                    public ICommand {methodName}Command => {lowMethodName} ??= ReactiveCommand.Create<{argumentTypeString},{returnTypeString}>({methodName}, Observable.Create<bool>(observer => 
                    {'{'}
                        observer.OnNext({canMethodName}());
                        return () => {'{'}{'}'};
                    {'}'}));
                """;
        return code;
    }

    string ICodeProvider.CreateClassBodyString()
    {
        return default!;
    }
}
