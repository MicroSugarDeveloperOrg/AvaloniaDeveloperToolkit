namespace Prism.SourceGenerators.Builder;

internal class CodeBuilder : IDisposable
{
    private CodeBuilder(string className, string nameSpace)
    {
        _className = className;
        _nameSpace = nameSpace;
    }

    string _className;
    string _nameSpace;
    string? _baseTypeName;
    List<string> _namespaces = [];
    List<string> _interfaceNames = [];
    Dictionary<string, (string type, string field)> _mapPropertyNames = [];
    Dictionary<string, (string? argType, string? can)> _mapMethodNames = [];

    public static CodeBuilder CreateBuilder(string className, string nameSpace) => new CodeBuilder(className, nameSpace);

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

    public bool AppendCommand(string? argumentType, string methodName, string? canMethodName)
    {
        if (_mapMethodNames.ContainsKey(methodName))
            return true;

        _mapMethodNames.Add(methodName, (argumentType, canMethodName));
        return true;
    }

    public string? Build()
    {
        if (string.IsNullOrWhiteSpace(_className))
            return default;

        //先写NameSpace
        var code =
            $"""
            {BuildNameSpaces()}
            namespace {_nameSpace};

            partial class {BuildClassName()}
            {'{'} 
            {BuildProperties()}
            {BuildCommands()}
            {'}'}
            """;

        return code;
    }

    public bool Clear()
    {
        _namespaces.Clear();
        _namespaces = null!;

        _interfaceNames.Clear();
        _interfaceNames = null!;

        _mapPropertyNames.Clear();
        _mapPropertyNames = null!;

        _mapMethodNames.Clear();
        _mapMethodNames = null!;

        return true;
    }

    string BuildNameSpaces()
    {
        StringBuilder builder = new();
        foreach (var item in _namespaces)
            builder.AppendLine($"using {item};");

        return builder.ToString();
    }

    string BuildClassName() => string.IsNullOrWhiteSpace(_baseTypeName) ? _className : $"{_className} : {_baseTypeName}";

    string BuildProperties()
    {
        StringBuilder builder = new();
        foreach (var item in _mapPropertyNames)
        {
            builder.Append(BuildProperty(item.Value.type, item.Value.field, item.Key));
            builder.AppendLine();
        }

        return builder.ToString();
    }

    string? BuildProperty(string type, string fieldName, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(fieldName) || string.IsNullOrWhiteSpace(propertyName))
            return default;

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
                        RaisePropertyChanged();
                        {propertyName}Changed(@old, @new);
                        {propertyName}Changed(@new);
                    {'}'}
                {'}'}

                partial void {propertyName}Changing({type} oldValue);
                partial void {propertyName}Changing({type} oldValue, {type} newValue);
                partial void {propertyName}Changed({type} oldValue, {type} newValue);
                partial void {propertyName}Changed({type} newValue);
            """;
        //var code = 
        //    $"""
        //        public {type} {propertyName} 
        //        {'{'} 
        //            get => {fieldName};
        //            set => SetProperty(ref _title, value, ({oldString},{newString}) =>
        //            {'{'} 
        //                {propertyName}Changing({oldString});
        //                {propertyName}Changing({oldString}, {newString});
        //            {'}'}, (@old, @new) =>
        //            {'{'} 
        //                {propertyName}Changed({oldString}, {newString});
        //                {propertyName}Changed({oldString});
        //            {'}'});
        //        {'}'}

        //        partial void {propertyName}Changing({type} oldValue);
        //        partial void {propertyName}Changing({type} oldValue, {type} newValue);
        //        partial void {propertyName}Changed({type} oldValue, {type} newValue);
        //        partial void {propertyName}Changed({type} newValue);
        //    """;
        return code;
    }

    string BuildCommands()
    {
        StringBuilder builder = new();
        foreach (var item in _mapMethodNames)
        {
            builder.Append(BuildCommand(item.Value.argType, item.Key, item.Value.can));
            builder.AppendLine();
        }

        return builder.ToString();
    }

    string? BuildCommand(string? argumentType, string methodName, string? canMethodName)
    {
        if (string.IsNullOrWhiteSpace(methodName))
            return default;

        var commandString = "Command";
        var arguments = string.IsNullOrWhiteSpace(canMethodName) ? $"{methodName}" : $"{methodName}, {canMethodName}";

        string code;
        if (string.IsNullOrWhiteSpace(argumentType))
        {
            code =
            $"""
                DelegateCommand _{methodName}{commandString};
                public ICommand {methodName}{commandString} => _{methodName}{commandString} ??= new DelegateCommand({arguments});
            """;
        }
        else
        {
            code =
           $"""
                DelegateCommand<{argumentType}> _{methodName}{commandString};
                public ICommand {methodName}{commandString} => _{methodName}{commandString} ??= new DelegateCommand<{argumentType}>({arguments});
            """;
        }

        return code;
    }

    public void Dispose()
    {
        Clear();
    }
}
