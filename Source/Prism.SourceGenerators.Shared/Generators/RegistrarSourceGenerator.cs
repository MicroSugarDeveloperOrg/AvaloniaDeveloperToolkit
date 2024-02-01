using Prism.Ioc;
using Prism.SourceGenerators.Builder;
using SourceGeneratorToolkit.Builders;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using SourceGeneratorToolkit.SyntaxContexts;
using static Prism.SourceGenerators.Helpers.CodeHelpers;
using static SourceGeneratorToolkit.Helpers.CommonHelpers;

namespace Prism.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class RegistrarSourceGenerator : ISourceGenerator, ICodeXProvider
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(typeof(RegistrarAttribute).Name, __GeneratorCSharpFileHeader__));
        context.RegisterForSyntaxNotifications(() => new RegistrarSyntaxContextReceiver(typeof(RegistrarAttribute).Name, typeof(ManyScopedAttribute<>).Name,
                                                                                                                         typeof(ManySingletonAttribute<>).Name,
                                                                                                                         typeof(ManyTransientAttribute<>).Name,
                                                                                                                         typeof(NavigationAttribute<>).Name,
                                                                                                                         typeof(NavigationAttribute<,>).Name,
                                                                                                                         typeof(ScopedAttribute).Name,
                                                                                                                         typeof(ScopedAttribute<>).Name,
                                                                                                                         typeof(ScopedAttribute<,>).Name,
                                                                                                                         typeof(SingletonAttribute).Name,
                                                                                                                         typeof(SingletonAttribute<>).Name,
                                                                                                                         typeof(SingletonAttribute<,>).Name,
                                                                                                                         typeof(TransientAttribute).Name,
                                                                                                                         typeof(TransientAttribute<>).Name,
                                                                                                                         typeof(TransientAttribute<,>).Name));
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxContextReceiver;
        if (receiver is not RegistrarSyntaxContextReceiver syntaxContextReceiver)
            return;

        var map = syntaxContextReceiver.GetRegistrations();
        if (map.mapAttributeDatas.Count <= 0)
            return;

        var recordRegistrarClass = map.registrarClass;
        if (recordRegistrarClass is null)
        {
            foreach (var mapAttributeData in map.mapAttributeDatas)
            {
                var classSymbol = mapAttributeData.Key;
                if (classSymbol is null) continue;

                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidRegistrarIocError<RegistrarSourceGenerator>(__Registrar__),
                                         classSymbol.Locations.FirstOrDefault(),
                                         classSymbol.Name,
                                         mapAttributeData.Value.AttributeClass?.MetadataName));
            }
        }
        else
        {
            var registrarClass = recordRegistrarClass.ClassSymbol;
            var registrarMethod = recordRegistrarClass.RegisterMethodSymbol;

            string? parameterName = default;
            foreach (var parameterSymbol in registrarMethod.Parameters)
            {
                if (parameterSymbol.Type.ToDisplayString() == __PrismContainerFull__)
                {
                    parameterName = parameterSymbol.Name;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(parameterName))
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidIocPartialMethodSignatureError<RegistrarSourceGenerator>(__PrismIocHeader__, __PrismContainer__),
                                        registrarClass.Locations.FirstOrDefault(),
                                        registrarClass.Name,
                                        registrarMethod.Name));
                return;
            }

            using CodeBuilderX builder = CodeBuilderX.CreateBuilderX(registrarClass.ContainingNamespace.ToDisplayString(), registrarClass.Name, this);
            builder.AppendUseIocSystemNameSpace();
            builder.AppendMethod(registrarMethod.Name, registrarMethod.CreateParameter(), registrarMethod.ReturnType.ToDisplayString());

            foreach (var mapAttributeData in map.mapAttributeDatas)
            {
                var classSymbol = mapAttributeData.Key;
                var attributeData = mapAttributeData.Value;
                if (classSymbol is null || attributeData is null) continue;

                var attributeClass = attributeData.AttributeClass;
                if (attributeClass is null) continue;

                var metaName = attributeClass.MetadataName;
                if (metaName == typeof(SingletonAttribute).Name)
                    builder.Singleton(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(SingletonAttribute<>).Name)
                    builder.SingletonT(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(SingletonAttribute<,>).Name)
                    builder.SingletonTT(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(ScopedAttribute).Name)
                    builder.Scoped(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(ScopedAttribute<>).Name)
                    builder.ScopedT(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(ScopedAttribute<,>).Name)
                    builder.ScopedTT(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(TransientAttribute).Name)
                    builder.Transient(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(TransientAttribute<>).Name)
                    builder.TransientT(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(TransientAttribute<,>).Name)
                    builder.TransientTT(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(NavigationAttribute<>).Name)
                    builder.NavigationT(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(NavigationAttribute<,>).Name)
                    builder.NavigationTT(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(ManySingletonAttribute<>).Name)
                    builder.ManySingleton(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(ManyScopedAttribute<>).Name)
                    builder.ManyScoped(parameterName!, registrarMethod.Name, attributeData);
                else if (metaName == typeof(ManyTransientAttribute<>).Name)
                    builder.ManyTransient(parameterName!, registrarMethod.Name, attributeData);
            }

            context.AddSource($"{registrarClass.Name}_{__Registrar__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }

    }

    string ICodeXProvider.CreateIocString(InjectType type, string containerName, string? token, string from, string to)
    {
        var tokenString = string.IsNullOrWhiteSpace(token) ? "" : $"\"{token}\"";
        switch (type)
        {
            case InjectType.Singleton:
                return string.IsNullOrWhiteSpace(from) ? $"{containerName}.{__Singleton__}<{to}>({tokenString})"
                                                       : $"{containerName}.{__Singleton__}<{from},{to}>({tokenString})";
            case InjectType.Scoped:
                return string.IsNullOrWhiteSpace(from) ? $"{containerName}.{__Scoped__}<{to}>()"
                                                       : $"{containerName}.{__Scoped__}<{from},{to}>()";
            case InjectType.Transient:
                return string.IsNullOrWhiteSpace(from) ? $"{containerName}.{__Transient__}<{to}>({tokenString})"
                                                       : $"{containerName}.{__Transient__}<{from},{to}>({tokenString})";
            case InjectType.Navigation:
            default:
                break;
        }

        return default!;
    }

    string ICodeXProvider.CreateManyIocString(InjectType type, string containerName, string? token, string to, params string[] froms)
    {
        List<string> lists = new();
        foreach (var item in froms)
        {
            if (string.IsNullOrWhiteSpace(item))
                continue;
            lists.Add($"typeof({item})");
        }
        var str = string.Join(",", lists);
        switch (type)
        {
            case InjectType.Singleton:
            case InjectType.Scoped:
                return $"{containerName}.{__ManySingleton__}<{to}>({str})";
            case InjectType.Transient:
                return $"{containerName}.{__ManyTransient__}<{to}>({str})";
            case InjectType.Navigation:
                break;
            default:
                break;
        }

        return default!;
    }

    string ICodeXProvider.CreateNavigationString(string containerName, string? token, string view, string viewModel)
    {
        var tokenString = string.IsNullOrWhiteSpace(token) ? "" : $"\"{token}\"";
        if (string.IsNullOrWhiteSpace(viewModel))
            return $"{containerName}.{__Navigation__}<{view}>({tokenString})";

        return $"{containerName}.{__Navigation__}<{view},{viewModel}>({tokenString})";
    }

    string ICodeProvider.GetRaisePropertyString(string fieldName, string propertyName)
    {
        throw new NotImplementedException();
    }

    string ICodeProvider.CreateCommandString(string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        throw new NotImplementedException();
    }
}
