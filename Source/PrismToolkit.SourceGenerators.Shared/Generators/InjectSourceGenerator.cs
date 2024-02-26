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
public class InjectSourceGenerator : ISourceGenerator, ICodeXProvider
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => 
        {
            //Debugger.Launch();
            context.CreateSourceCodeFromEmbeddedResource(__InjectAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__ManyInjectAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__);
            
            context.CreateSourceCodeFromEmbeddedResource(__NavigationAttributeEmbeddedResourceNameT__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__NavigationAttributeEmbeddedResourceNameTT__, __GeneratorCSharpFileHeader__);
            
            context.CreateSourceCodeFromEmbeddedResource(__ScopedAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__ScopedAttributeEmbeddedResourceNameT__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__ScopedAttributeEmbeddedResourceNameTT__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__ManyScopedAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__);

            context.CreateSourceCodeFromEmbeddedResource(__SingletonAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__SingletonAttributeEmbeddedResourceNameT__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__SingletonAttributeEmbeddedResourceNameTT__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__ManySingletonAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__);

            context.CreateSourceCodeFromEmbeddedResource(__TransientAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__TransientAttributeEmbeddedResourceNameT__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__TransientAttributeEmbeddedResourceNameTT__, __GeneratorCSharpFileHeader__);
            context.CreateSourceCodeFromEmbeddedResource(__ManyTransientAttributeEmbeddedResourceName__, __GeneratorCSharpFileHeader__);
        });

        context.RegisterForSyntaxNotifications(() => new InjectSyntaxContextReceiver(typeof(ManyScopedAttribute<>).Name,
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
        if (receiver is not InjectSyntaxContextReceiver syntaxContextReceiver)
            return;

        var map = syntaxContextReceiver.GetInjects();
        if (map.Count <= 0)
            return;

        foreach (var mapClass in map)
        {
            var classSymbol = mapClass.Key;
            var mapMethods = mapClass.Value;

            using CodeBuilderX builder = CodeBuilderX.CreateBuilderX(classSymbol.ContainingNamespace.ToDisplayString(), classSymbol.Name, classSymbol.IsAbstract, this);
            builder.AppendUseIocSystemNameSpace();

            foreach (var mapMethod in mapMethods)
            {
                var recordMethod = mapMethod.Value;
                var methodSymbol = recordMethod.MethodSymbol;
                var attributeDatas = recordMethod.AttributeDatas;

                if (attributeDatas.Length <= 0)
                    continue;

                //Debugger.Launch();
                if (methodSymbol.Parameters.Length < 1)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidIocPartialMethodSignatureError<InjectSourceGenerator>(__PrismIocHeader__, __PrismContainer__),
                                             methodSymbol.Locations.FirstOrDefault(),
                                             classSymbol.Name,
                                             methodSymbol.Name));

                    continue;
                }

                string? parameterName = default;
                foreach (var parameterSymbol in methodSymbol.Parameters)
                {
                    if (parameterSymbol.Type.ToDisplayString() == __PrismContainerFull__)
                    {
                        parameterName = parameterSymbol.Name;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(parameterName))
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateInvalidIocPartialMethodSignatureError<InjectSourceGenerator>(__PrismIocHeader__, __PrismContainer__),
                                            methodSymbol.Locations.FirstOrDefault(),
                                            classSymbol.Name,
                                            methodSymbol.Name));
                    continue;
                }

                //builder.
                builder.AppendMethod(methodSymbol.Name, methodSymbol.CreateParameter(), methodSymbol.ReturnType.ToDisplayString());
                //Debugger.Launch();
                foreach (var attributeData in attributeDatas)
                {
                    var attributeClass = attributeData.AttributeClass;
                    if (attributeClass is null)
                        continue;
                    var metaName = attributeClass.MetadataName;
                    if (metaName == typeof(SingletonAttribute).Name)
                        builder.Singleton(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(SingletonAttribute<>).Name)
                        builder.SingletonT(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(SingletonAttribute<,>).Name)
                        builder.SingletonTT(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(ScopedAttribute).Name)
                        builder.Scoped(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(ScopedAttribute<>).Name)
                        builder.ScopedT(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(ScopedAttribute<,>).Name)
                        builder.ScopedTT(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(TransientAttribute).Name)
                        builder.Transient(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(TransientAttribute<>).Name)
                        builder.TransientT(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(TransientAttribute<,>).Name)
                        builder.TransientTT(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(NavigationAttribute<>).Name)
                        builder.NavigationT(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(NavigationAttribute<,>).Name)
                        builder.NavigationTT(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(ManySingletonAttribute<>).Name)
                        builder.ManySingleton(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(ManyScopedAttribute<>).Name)
                        builder.ManyScoped(parameterName!, methodSymbol.Name, attributeData);
                    else if (metaName == typeof(ManyTransientAttribute<>).Name)
                        builder.ManyTransient(parameterName!, methodSymbol.Name, attributeData);
                }
            }

            context.AddSource($"{classSymbol.Name}_{__Registration__}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
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

    string ICodeXProvider.CreateMethodString(string? argumentType, string? returnType, string methodName)
    {
        return default!;
    }

    string ICodeProvider.CreateClassBodyString()
    {
        return default!;
    }
}
