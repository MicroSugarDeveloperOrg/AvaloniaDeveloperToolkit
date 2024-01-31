using Microsoft.CodeAnalysis;
using Prism.SourceGenerators.Helpers;
using SourceGeneratorToolkit.Builders;
using SourceGeneratorToolkit.Extensions;

namespace Prism.SourceGenerators.Builder;
internal static class CodeBuildXExtensions
{
    public static (string? from, string? to) GetTypeArguments(ImmutableArray<ITypeSymbol> typeArguments)
    {
        if (typeArguments.Length <= 0)
            return (default, default);

        if (typeArguments.Length == 1)
            return (default, typeArguments.First().ToDisplayString());

        return (typeArguments[0].ToDisplayString(), typeArguments[1].ToDisplayString());
    }

    public static (string? view, string? viewModel) GetNavigationTypeArguments(ImmutableArray<ITypeSymbol> typeArguments)
    {
        if (typeArguments.Length <= 0)
            return (default, default);

        if (typeArguments.Length == 1)
            return (typeArguments.First().ToDisplayString(), default);
        
        return (typeArguments[0].ToDisplayString(), typeArguments[1].ToDisplayString());
    }

    public static (string? from, string? to) GetParameterArguments(ImmutableArray<TypedConstant> constructorArguments, ImmutableArray<IParameterSymbol> constructorParameters)
    {
        if (constructorArguments.Length != constructorParameters.Length)
            return (default, default);

        string? from = default;
        string? to = default;
        for (var i = 0; i < constructorArguments.Length; i++)
        {
            if (constructorParameters[i].Name == CodeHelpers.__AttributeConstructorParameter_From__)
            {
                from = constructorArguments[i].Value?.ToString();
                continue;
            }

            if (constructorParameters[i].Name == CodeHelpers.__AttributeConstructorParameter_To__)
            {
                to = constructorArguments[i].Value?.ToString();
                continue;
            }
        }

        return (from, to);
    }

    public static (string? from, string? to, string? token) GetNamedArguments(ImmutableArray<KeyValuePair<string, TypedConstant>> namedArguments)
    {
        string? from = default;
        string? to = default;
        string? token = default;

        foreach (var argument in namedArguments)
        {
            if (argument.Key == CodeHelpers.__AttributeConstructorNamedArgument_From__)
            {
                from = argument.Value.ToString();
                continue;
            }

            if (argument.Key == CodeHelpers.__AttributeConstructorNamedArgument_To__)
            {
                to = argument.Value.ToString();
                continue;
            }

            if (argument.Key == CodeHelpers.__AttributeConstructorNamedArgument_Token__)
            {
                token = argument.Value.Value?.ToString();
                continue;
            }
        }

        return (from, to, token);
    }

    public static ImmutableArray<string> GetFromsFromConstrutorArguments(ImmutableArray<TypedConstant> constructorArguments)
    {
        List<string> lists = new();
        foreach (var item in constructorArguments.First().Values)
            lists.Add(item.Value!.ToString());

        return lists.ToImmutableArray();
    }

    public static string? GetToken(ImmutableArray<KeyValuePair<string, TypedConstant>> namedArguments)
    {
        foreach (var argument in namedArguments)
        {
            if (argument.Key == CodeHelpers.__AttributeConstructorNamedArgument_Token__)
                return argument.Value.Value?.ToString();
        }

        return default;
    }

    public static CodeBuilderX Singleton(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        string? from = default; 
        string? to = default;
        string? token = default;

        var constructorArguments = attributeData.ConstructorArguments;
        var constructor = attributeClass.Constructors.FirstOrDefault(constructor => constructor.Parameters.Length == constructorArguments.Length);
        if (constructor != null)
        {
            var constructorParameters = constructor.Parameters;
            var value = GetParameterArguments(constructorArguments, constructorParameters);
            from = value.from;
            to = value.to;
        }

        var namedValue = GetNamedArguments(attributeData.NamedArguments);
        if (!string.IsNullOrWhiteSpace(namedValue.from))
            from = namedValue.from;

        if (!string.IsNullOrWhiteSpace(namedValue.to))
            to = namedValue.to;

        token = namedValue.token;

        var element = MethodElement.CreateIocElement(methodName, containerName, InjectType.Singleton, token, from, to!);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX SingletonT(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder;

        if (attributeData is null)
            return builder;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder;

        var value = GetTypeArguments(attributeClass.TypeArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateIocElement(methodName, containerName, InjectType.Singleton, token, value.from, value.to!);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX SingletonTT(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder;

        if (attributeData is null)
            return builder;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder;

        var value = GetTypeArguments(attributeClass.TypeArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateIocElement(methodName, containerName, InjectType.Singleton, token, value.from, value.to!);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX Scoped(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder;

        if (attributeData is null)
            return builder;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder;


        string? from = default;
        string? to = default;
        string? token = default;

        var constructorArguments = attributeData.ConstructorArguments;
        var constructor = attributeClass.Constructors.FirstOrDefault(constructor => constructor.Parameters.Length == constructorArguments.Length);
        if (constructor != null)
        {
            var constructorParameters = constructor.Parameters;
            var value = GetParameterArguments(constructorArguments, constructorParameters);
            from = value.from;
            to = value.to;
        }

        var namedValue = GetNamedArguments(attributeData.NamedArguments);
        if (!string.IsNullOrWhiteSpace(namedValue.from))
            from = namedValue.from;

        if (!string.IsNullOrWhiteSpace(namedValue.to))
            to = namedValue.to;

        token = namedValue.token;

        var element = MethodElement.CreateIocElement(methodName, containerName, InjectType.Scoped, token, from, to!);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX ScopedT(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        var value = GetTypeArguments(attributeClass.TypeArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateIocElement(methodName, containerName, InjectType.Scoped, token, value.from, value.to!);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX ScopedTT(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        var value = GetTypeArguments(attributeClass.TypeArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateIocElement(methodName, containerName, InjectType.Scoped, token, value.from, value.to!);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX Transient(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        string? from = default;
        string? to = default;
        string? token = default;

        var constructorArguments = attributeData.ConstructorArguments;
        var constructor = attributeClass.Constructors.FirstOrDefault(constructor => constructor.Parameters.Length == constructorArguments.Length);
        if (constructor != null)
        {
            var constructorParameters = constructor.Parameters;
            var value = GetParameterArguments(constructorArguments, constructorParameters);
            from = value.from;
            to = value.to;
        }

        var namedValue = GetNamedArguments(attributeData.NamedArguments);
        if (!string.IsNullOrWhiteSpace(namedValue.from))
            from = namedValue.from;

        if (!string.IsNullOrWhiteSpace(namedValue.to))
            to = namedValue.to;

        token = namedValue.token;

        var element = MethodElement.CreateIocElement(methodName, containerName, InjectType.Transient, token, from, to!);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX TransientT(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        var value = GetTypeArguments(attributeClass.TypeArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateIocElement(methodName, containerName, InjectType.Transient, token, value.from, value.to!);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX TransientTT(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        var value = GetTypeArguments(attributeClass.TypeArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateIocElement(methodName, containerName, InjectType.Transient, token, value.from, value.to!);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX NavigationT(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        var value = GetNavigationTypeArguments(attributeClass.TypeArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateNavigationElement(methodName, containerName, token, value.view!, value.viewModel);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX NavigationTT(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        var value = GetNavigationTypeArguments(attributeClass.TypeArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateNavigationElement(methodName, containerName, token, value.view!, value.viewModel);
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX ManySingleton(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder;

        if (attributeData is null)
            return builder;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder;

        var to = attributeClass.TypeArguments.First().ToDisplayString();
        var froms = GetFromsFromConstrutorArguments(attributeData.ConstructorArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateManyIocElement(methodName, containerName, InjectType.Singleton, token, to, froms.ToArray());
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX ManyScoped(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        var to = attributeClass.TypeArguments.First().ToDisplayString();
        var froms = GetFromsFromConstrutorArguments(attributeData.ConstructorArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateManyIocElement(methodName, containerName, InjectType.Scoped, token, to, froms.ToArray());
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }

    public static CodeBuilderX ManyTransient(this CodeBuilderX builder, string containerName, string methodName, AttributeData attributeData)
    {
        if (builder is null)
            return builder!;

        if (string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(methodName))
            return builder!;

        if (attributeData is null)
            return builder!;

        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
            return builder!;

        var to = attributeClass.TypeArguments.First().ToDisplayString();
        var froms = GetFromsFromConstrutorArguments(attributeData.ConstructorArguments);
        var token = GetToken(attributeData.NamedArguments);

        var element = MethodElement.CreateManyIocElement(methodName, containerName, InjectType.Transient, token, to, froms.ToArray());
        if (element is null)
            return builder;

        return builder.AppendMethodElementX(methodName, element);
    }
}