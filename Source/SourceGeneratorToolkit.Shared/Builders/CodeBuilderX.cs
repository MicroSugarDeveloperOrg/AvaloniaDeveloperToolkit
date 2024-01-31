namespace SourceGeneratorToolkit.Builders;

internal class MethodElement(string methodName, string containerName, InjectType injectType, string? token, bool isMany, string to, params string[] froms)
{
    public static MethodElement? CreateIocElement(string methodName, string containerName, InjectType injectType, string? token, string? from, string to)
    {
        if (string.IsNullOrWhiteSpace(methodName) || string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(to))
            return default;

        return new MethodElement(methodName, containerName, injectType, token, false, to, from!);
    }

    public static MethodElement? CreateManyIocElement(string methodName, string containerName, InjectType injectType, string? token, string to, params string[] froms)
    {
        if (string.IsNullOrWhiteSpace(methodName) || string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(to))
            return default;

        if (froms.Length <= 0)
            return default;

        return new MethodElement(methodName, containerName, injectType, token, true, to, froms);
    }

    public static MethodElement? CreateNavigationElement(string methodName, string containerName, string? token, string view, string? viewModel)
    {
        if (string.IsNullOrWhiteSpace(methodName) || string.IsNullOrWhiteSpace(containerName) || string.IsNullOrWhiteSpace(view))
            return default;

        return new MethodElement(methodName, containerName, InjectType.Navigation, token, false, view, viewModel!);
    }

    public string MethodName => methodName;

    public string ContainerName => containerName;   

    public InjectType InjectType => injectType;

    public string? Token => token;

    public bool IsMany => isMany;   

    public string To => to;

    public string[] Froms => froms;
}


internal class CodeBuilderX : CodeBuilder
{
    protected CodeBuilderX(string nameSpace, string className, ICodeXProvider? provider)
        : base(nameSpace, className, provider)
    {
        _provider = provider;
    }

    ICodeXProvider? _provider;

    protected Dictionary<string, (string? argument, string? returnType)> _mapMethods = new();
    protected Dictionary<string, List<MethodElement>> _mapMethodElements = new();

    public static CodeBuilderX CreateBuilderX(string nameSpace, string className, ICodeXProvider? provider) => new CodeBuilderX(nameSpace, className, provider);

    public bool AppendMethod(string methodName, string? argument, string? returnType)
    {
        if (string.IsNullOrWhiteSpace(methodName))
            return false;

        _mapMethods[methodName] = (argument, returnType);
        return true;
    }

    public bool AppendMethodElement(string methodName, MethodElement element)
    {
        if (string.IsNullOrWhiteSpace(methodName))
            return false;

        if (element is null)
            return false;

        _mapMethodElements.TryGetValue(methodName, out var elements);
        if (elements == null)
        {
            elements = new List<MethodElement>();
            _mapMethodElements[methodName] = elements;
        }

        elements.Add(element);
        return true;
    }


    public override string? Build()
    {
        if (string.IsNullOrWhiteSpace(_className))
            return default;

        //先写NameSpace
        var code =
            $"""
            {BuildNameSpaces()}
            namespace {_nameSpace};

            #nullable enable
            partial class {BuildClassName()}
            {'{'} 
            {BuildProperties()}
            {BuildCommands()}
            {BuildMethods()}
            {'}'}
            #nullable disable
            """;

        return code;
    }

    protected string BuildMethods()
    {
        StringBuilder builder = new();
        foreach (var mapMethod in _mapMethods)
        {
            builder.Append(BuildMethod(mapMethod.Key, mapMethod.Value.argument, mapMethod.Value.returnType));
            builder.AppendLine();
        }

        return builder.ToString();
    }

    protected string? BuildMethod(string methodName, string? argument, string? returnType)
    {
        if (string.IsNullOrWhiteSpace(methodName))
            return default;

        var returnTypeString = string.IsNullOrWhiteSpace(returnType) ? "void" : returnType;
        _mapMethodElements.TryGetValue(methodName, out var elements);
        if (elements == null)
            return default;

        StringBuilder builder = new();
        builder.AppendLine();
        foreach (var element in elements)
        {
            builder.Append(BuildMethodElement(element));
            builder.AppendLine();
        }

        var code =
            $"""
                partial {returnTypeString} {methodName}({argument})
                {'{'}
                {builder}
                {'}'}
            """;
        return code;
    }
 
    protected string? BuildMethodElement(MethodElement element)
    {
        if (element is null)
            return default;

        string? code = default;
        switch (element.InjectType)
        {
            case InjectType.Singleton:
            case InjectType.Scoped:
            case InjectType.Transient:
                code = element.IsMany ? _provider?.CreateManyIocString(element.InjectType, element.ContainerName, element.Token, element.To, element.Froms)
                                      : _provider?.CreateIocString(element.InjectType, element.ContainerName, element.Token, element.Froms.FirstOrDefault(), element.To);
                break;
            case InjectType.Navigation:
                code = _provider?.CreateNavigationString(element.ContainerName, element.Token, element.To, element.Froms.FirstOrDefault());
                break;
            default:
                break;
        }

        return $"""
                    {code};
            """;
    }

    public override bool Clear()
    {
        _mapMethods.Clear();
        foreach (var item in _mapMethodElements)
            item.Value.Clear();
        _mapMethodElements.Clear();
        return base.Clear();
    }
}
