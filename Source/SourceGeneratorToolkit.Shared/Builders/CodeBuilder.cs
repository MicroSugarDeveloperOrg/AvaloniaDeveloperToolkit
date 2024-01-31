namespace SourceGeneratorToolkit.Builders;

internal class CodeBuilder : IDisposable
{
    protected CodeBuilder(string nameSpace, string className, ICodeProvider? provider)
    {
        _className = className;
        _nameSpace = nameSpace;

        _provider = provider;
    }

    ICodeProvider? _provider;

    protected string _className;
    protected string _nameSpace;
    protected string? _baseTypeName;
    protected string _commandString = string.Empty;
    protected string _raisePropertyString = string.Empty;
    protected List<string> _namespaces = [];
    protected List<string> _interfaceNames = [];
    protected Dictionary<string, (string type, string field)> _mapPropertyNames = [];
    protected Dictionary<string, (string? argType, string? returnType, string? can)> _mapCommandNames = [];

    public static CodeBuilder CreateBuilder(string nameSpace, string className,ICodeProvider? provider) => new CodeBuilder(nameSpace, className, provider);

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
            {'}'}
            #nullable disable
            """;

        return code;
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

    protected string BuildClassName() => string.IsNullOrWhiteSpace(_baseTypeName) ? _className : $"{_className} : {_baseTypeName}";

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

    protected string BuildCommands()
    {
        StringBuilder builder = new();
        foreach (var item in _mapCommandNames)
        {
            builder.Append(BuildCommand(item.Value.argType, item.Value.returnType , item.Key, item.Value.can));
            builder.AppendLine();
        }

        return builder.ToString();
    }

    protected string? BuildCommand(string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        if (string.IsNullOrWhiteSpace(methodName))
            return default;

        //var commandString = "Command";
        //var arguments = string.IsNullOrWhiteSpace(canMethodName) ? $"{methodName}" : $"{methodName}, {canMethodName}";

        return _provider?.CreateCommandString(argumentType, returnType, methodName, canMethodName);
    }

    public void Dispose()
    {
        Clear();
    }
}
