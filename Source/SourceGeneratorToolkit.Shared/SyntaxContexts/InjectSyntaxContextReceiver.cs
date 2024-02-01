namespace SourceGeneratorToolkit.SyntaxContexts;

internal sealed class RecordInjectMethod(IMethodSymbol _methodSymbol, MethodDeclarationSyntax _methodSyntax) : IEqualityComparer<RecordInjectMethod>
{
    public string Name => _methodSymbol.ToDisplayString();

    public ImmutableArray<AttributeData> AttributeDatas { get; init; } = ImmutableArray.Create<AttributeData>();

    public IMethodSymbol MethodSymbol => _methodSymbol;

    public MethodDeclarationSyntax MethodSyntax => _methodSyntax;

    public static bool operator ==(RecordInjectMethod left, RecordInjectMethod right) => left.Equals(right);

    public static bool operator !=(RecordInjectMethod left, RecordInjectMethod right) => !left.Equals(right);

    public bool Equals(RecordInjectMethod x, RecordInjectMethod y)
    {
        if (x is null || y is null)
            return false;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        if (ReferenceEquals(x, y))
            return true;

        return x.Name == y.Name;
    }

    public int GetHashCode(RecordInjectMethod obj)
    {
        if (obj is null)
            return 0;

        return obj.Name.GetHashCode();
    }

    public override bool Equals(object obj) => Equals(this, obj);

    public override int GetHashCode() => GetHashCode(this);
}

internal sealed class InjectSyntaxContextReceiver : ISyntaxContextReceiver
{
    public InjectSyntaxContextReceiver(params string[] injectStrings)
    {
        _injectStrings = injectStrings.ToImmutableArray();
    }

    ImmutableArray<string> _injectStrings;

    Dictionary<INamedTypeSymbol, Dictionary<string, RecordInjectMethod>> _mapInjects = [];

    void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclarationSyntax || methodDeclarationSyntax.AttributeLists.Count <= 0)
            return;

        var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
        if (methodSymbol is null)
            return;

        var attributes = methodSymbol.GetAttributes();

        List<AttributeData> attributesDatas = new();
        foreach (var attributeData in attributes)
        {
            var attributeClass = attributeData.AttributeClass;
            if (attributeClass is null)
                continue;

            var metaDataName = attributeClass.MetadataName;
            if (_injectStrings.Contains(metaDataName))
                attributesDatas.Add(attributeData);
        }

        if (attributesDatas.Count <= 0)
            return;

        _mapInjects.TryGetValue(methodSymbol.ContainingType, out var mapMethods);
        if (mapMethods is null)
        {
            mapMethods = [];
            _mapInjects[methodSymbol.ContainingType] = mapMethods;
        }

        RecordInjectMethod recordMethod = new(methodSymbol, methodDeclarationSyntax)
        {
            AttributeDatas = attributesDatas.ToImmutableArray(),
        };
        mapMethods[methodSymbol.ToDisplayString()] = recordMethod;
    }

    public ImmutableDictionary<INamedTypeSymbol, ImmutableDictionary<string, RecordInjectMethod>> GetInjects()
    {
        Dictionary<INamedTypeSymbol, ImmutableDictionary<string, RecordInjectMethod>> mapInjects = [];

        foreach (var mapInjectKeyValue in _mapInjects)
            mapInjects[mapInjectKeyValue.Key] = mapInjectKeyValue.Value.ToImmutableDictionary();

        return mapInjects.ToImmutableDictionary(SymbolEqualityComparer.Default);
    }

    public bool Clear()
    {
        foreach (var mapKeyValue in _mapInjects)
        {
            foreach (var mapMethodKeyValue in mapKeyValue.Value)
                mapMethodKeyValue.Value.AttributeDatas.Clear();

            mapKeyValue.Value.Clear();
        }

        _mapInjects.Clear();
        _mapInjects = null!;
        return true;
    }
}
