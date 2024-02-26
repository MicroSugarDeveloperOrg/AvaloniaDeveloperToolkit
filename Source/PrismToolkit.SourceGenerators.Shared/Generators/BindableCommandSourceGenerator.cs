using Prism.Commands;
using Prism.SourceGenerators.Builder;
using SourceGeneratorToolkit.Builders;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using SourceGeneratorToolkit.SyntaxContexts;
using static Prism.SourceGenerators.Helpers.CodeHelpers;
using static SourceGeneratorToolkit.Helpers.CommonHelpers;

namespace Prism.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class BindableCommandSourceGenerator : ISourceGenerator, ICodeProvider
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(__BindableCommandAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__));
        context.RegisterForSyntaxNotifications(() => new CommandSyntaxContextReceiver(__BindableCommandFullAttribute__));
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

                if (!methodSymbol.ReturnsVoid || methodSymbol.Parameters.Length > 1)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidBindableCommandMethodSignatureError<BindableCommandSourceGenerator>(__BindableCommand__),
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
                                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidCanExecuteMemberNameError<BindableCommandSourceGenerator>(__BindableCommand__),
                                                         methodSymbol.Locations.FirstOrDefault(),
                                                         $"{classSymbol.Name}.{canMethodSymbol.Name}",
                                                         canMethodSymbol.ReturnType.Name));
                                continue;
                            }

                            var canMethodParameterSymbol = canMethodSymbol.Parameters.FirstOrDefault();
                            if (canMethodParameterSymbol is not null && canMethodParameterSymbol.Type.GetFullyQualifiedName() != parameterType)
                            {
                                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidCanExecuteMemberNameError<BindableCommandSourceGenerator>(__BindableCommand__),
                                                         methodSymbol.Locations.FirstOrDefault(),
                                                         $"{classSymbol.Name}.{canMethodSymbol.Name}",
                                                         canMethodSymbol.ReturnType.Name));
                                continue;
                            }
                        }
                    }
                }

                builder.AppendCommand(parameterType, methodSymbol.ReturnType.ToDisplayString(), methodSymbol.GetGeneratedMethodName(), canExcMethod);
            }

            context.AddSource($"{classSymbol.Name}_{__BindableCommand__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }
    }

    string ICodeProvider.GetRaisePropertyString(string fieldName, string propertyName)
    {
        throw new NotImplementedException();
    }

    string ICodeProvider.CreateCommandString(string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        var arguments = string.IsNullOrWhiteSpace(canMethodName) ? $"{methodName}" : $"{methodName}, {canMethodName}";
        var lowMethodName = $"_{methodName.FirstCharToLow()}Command";

        string code;
        if (string.IsNullOrWhiteSpace(argumentType))
        {
            code =
            $"""
                DelegateCommand? {lowMethodName};
                public ICommand {methodName}Command =>  {lowMethodName} ??= new DelegateCommand({arguments});
            """;
        }
        else
        {
            code =
           $"""
                DelegateCommand<{argumentType}>? {lowMethodName};
                public ICommand {methodName}Command => {lowMethodName} ??= new DelegateCommand<{argumentType}>({arguments});
            """;
        }

        return code;
    }

    string ICodeProvider.CreateClassBodyString()
    {
        return default!;
    }
}
