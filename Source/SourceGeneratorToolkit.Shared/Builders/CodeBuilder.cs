namespace SourceGeneratorToolkit.Builders;

internal class CodeBuilder : IDisposable
{
    protected CodeBuilder(string nameSpace, string className, bool isAbstract, ICodeProvider? provider)
    {
        _className = className;
        _nameSpace = nameSpace;
        _isAbstract= isAbstract;
        _provider = provider;
    }

    ICodeProvider? _provider;

    protected readonly string _nameSpace;
    protected readonly string _className;
    protected readonly bool _isAbstract;

    protected string? _baseTypeName;
    protected string _commandString = string.Empty;
    protected string _raisePropertyString = string.Empty;
    protected List<string> _namespaces = [];
    protected List<string> _interfaceNames = [];
    protected Dictionary<string, (string type, string field)> _mapPropertyNames = [];
    protected Dictionary<string, List<string>> _mapPropertyAttributes = [];
    protected Dictionary<string, (string? argType, string? returnType, string? can)> _mapCommandNames = [];

    public static CodeBuilder CreateBuilder(string nameSpace, string className, bool isAbstract, ICodeProvider? provider) 
        => new CodeBuilder(nameSpace, className, isAbstract, provider);

    public string CommandString
    {
        get => _commandString;
        set => _commandString = value;
    }

    public string RaisePropertyString
    {
        get => _raisePropertyString;
        set => _raisePropertyString = value;
    }

    public bool AppendUseNameSpace(string useNameSpace)
    {
        if (_namespaces.Contains(useNameSpace))
            return true;

        _namespaces.Add(useNameSpace);
        return true;
    }

    public bool AppendBaseType(string baseType)
    {
        _baseTypeName = baseType;
        return true;
    }

    public bool AppendInterface(string interfaceName)
    {
        if (_interfaceNames.Contains(interfaceName))
            return true;

        _interfaceNames.Add(interfaceName);
        return true;
    }

    public bool AppendProperty(string type, string fieldName, string propertyName)
    {
        if (_mapPropertyNames.ContainsKey(propertyName))
            return true;

        _mapPropertyNames.Add(propertyName, (type, fieldName));
        return true;
    }

    public bool AppendPropertyAttribute(string propertyName, string attributeDescription)
    {
        if (string.IsNullOrWhiteSpace(propertyName) || string.IsNullOrWhiteSpace(attributeDescription))
            return false;

        _mapPropertyAttributes.TryGetValue(propertyName, out var attributes);
        if (attributes is null)
        {
            attributes = [];
            _mapPropertyAttributes[propertyName] = attributes;
        }
        attributes.Add(attributeDescription);
        return true;
    }

    public bool AppendCommand(string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        if (_mapCommandNames.ContainsKey(methodName))
            return true;

        _mapCommandNames.Add(methodName, (argumentType, returnType, canMethodName));
        return true;
    }

    public virtual string? Build()
    {
        if (string.IsNullOrWhiteSpace(_className))
            return default;

        if (_mapPropertyNames.Count > 0 && _mapCommandNames.Count > 0)
        {
            return  $"""
            {BuildNameSpaces()}
            namespace {_nameSpace};

            #nullable enable
            partial {BuildAbstract()}class {BuildClassName()}
            {'{'} 
            {BuildProperties()}
            {BuildCommands()}
            {BuildClassBody()}
            {'}'}
            #nullable disable
            """;
        }

        if (_mapPropertyNames.Count > 0)
        {
            return $"""
            {BuildNameSpaces()}
            namespace {_nameSpace};

            #nullable enable
            partial {BuildAbstract()}class {BuildClassName()}
            {'{'} 
            {BuildProperties()} 
            {BuildClassBody()}
            {'}'}
            #nullable disable
            """;
        }

        if (_mapCommandNames.Count > 0)
        {
            return $"""
            {BuildNameSpaces()}
            namespace {_nameSpace};

            #nullable enable
            partial {BuildAbstract()}class {BuildClassName()}
            {'{'} 
            {BuildCommands()} 
            {BuildClassBody()}
            {'}'}
            #nullable disable
            """;
        }

        return $"""
            {BuildNameSpaces()}
            namespace {_nameSpace};

            #nullable enable
            partial {BuildAbstract()}class {BuildClassName()}
            {'{'} 
            {BuildClassBody()}
            {'}'}
            #nullable disable
            """;
    }

    public virtual bool Clear()
    {
        _namespaces.Clear();
        _namespaces = null!;

        _interfaceNames.Clear();
        _interfaceNames = null!;

        _mapPropertyNames.Clear();
        _mapPropertyNames = null!;

        _mapCommandNames.Clear();
        _mapCommandNames = null!;

        return true;
    }

    protected string BuildNameSpaces()
    {
        StringBuilder builder = new();
        foreach (var item in _namespaces)
            builder.AppendLine($"using {item};");

        return builder.ToString();
    }

    protected string BuildAbstract()
    {
        return _isAbstract ? "abstract " : "";
    }

    protected string BuildClassName()
    {
        bool isBaseType = !string.IsNullOrWhiteSpace(_baseTypeName);
        var classType = isBaseType ? $"{_className} : {_baseTypeName}" : _className;

        var interfaceString = string.Join(", ", _interfaceNames);
        if (!string.IsNullOrWhiteSpace(interfaceString))
        {
            if (isBaseType)
                classType = $"{classType},{interfaceString}";
            else
                classType = $"{classType} : {interfaceString}";
        }

        return classType;
    }

    protected string BuildProperties()
    {
        StringBuilder builder = new();
        foreach (var item in _mapPropertyNames)
        {
            builder.Append(BuildProperty(item.Value.type, item.Value.field, item.Key));
            builder.AppendLine();
        }

        return builder.ToString();
    }

    protected string? BuildProperty(string type, string fieldName, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(fieldName) || string.IsNullOrWhiteSpace(propertyName))
            return default;

        var raisePropertyString = _provider?.GetRaisePropertyString(fieldName, propertyName);
        var code =
            $"""
                {BuildPropertyAttributes(propertyName)}
                public {type} {propertyName}
                {'{'}
                    get => {fieldName};
                    set
                    {'{'}
                        if (EqualityComparer<{type}>.Default.Equals({fieldName}, value)) return;
                        var @old = {fieldName};
                        var @new = value;

                        {propertyName}Changing(@old);
                        {propertyName}Changing(@old, @new);
                        {raisePropertyString};
                        {propertyName}Changed(@old, @new);
                        {propertyName}Changed(@new);
                    {'}'}
                {'}'}

                partial void {propertyName}Changing({type} oldValue);
                partial void {propertyName}Changing({type} oldValue, {type} newValue);
                partial void {propertyName}Changed({type} oldValue, {type} newValue);
                partial void {propertyName}Changed({type} newValue);
            """;

        return code;
    }

    protected string? BuildPropertyAttributes(string propertyName)
    {
        _mapPropertyAttributes.TryGetValue(propertyName, out var attributes);
        if (attributes is null || attributes.Count <= 0)
            return default;

        StringBuilder builder = new();

        foreach (var item in attributes)
        {
            builder.AppendLine();
            builder.Append($"    [{item}]");
        }
        //builder.AppendLine();
        return builder.ToString();
    }

    protected string BuildCommands()
    {
        StringBuilder builder = new();
        int i = 0;

        foreach (var item in _mapCommandNames)
        {
            if (string.IsNullOrWhiteSpace(item.Key))
                continue;

            if (i > 0)
            {
                builder.AppendLine();
                builder.AppendLine();
            }
            builder.Append(BuildCommand(item.Value.argType, item.Value.returnType , item.Key, item.Value.can));
            i++;
        }

        return builder.ToString();
    }

    protected string? BuildCommand(string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        if (string.IsNullOrWhiteSpace(methodName))
            return default;

        return _provider?.CreateCommandString(argumentType, returnType, methodName, canMethodName);
    }

    protected string? BuildClassBody()
    {
        return _provider?.CreateClassBodyString();
    }

    public void Dispose()
    {
        Clear();
    }
}
