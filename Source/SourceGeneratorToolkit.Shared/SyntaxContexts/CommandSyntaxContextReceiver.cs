namespace SourceGeneratorToolkit.SyntaxContexts;

internal sealed class CommandSyntaxContextReceiver : ISyntaxContextReceiver
{
    public CommandSyntaxContextReceiver(string commandAttributeString)
    {
        CommandAttributeString = commandAttributeString;
    }

    Dictionary<INamedTypeSymbol, List<IMethodSymbol>> _mapMethods = [];

    public string CommandAttributeString { get; set; } = string.Empty;

    void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclarationSyntax || methodDeclarationSyntax.AttributeLists.Count <= 0)
            return;

        var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
        if (methodSymbol is null)
            return;

        //Debugger.Launch();
        if (!methodSymbol.GetAttributes().Any(ad => ad.AttributeClass?.ToDisplayString() == CommandAttributeString))
            return;

        var type = methodSymbol.ContainingType;
        _mapMethods.TryGetValue(type, out var value);
        if (value is null)
        {
            value = new List<IMethodSymbol>();
            _mapMethods.Add(type, value);
        }

        value.Add(methodSymbol);
    }

    public ImmutableDictionary<INamedTypeSymbol, ImmutableArray<IMethodSymbol>> GetMethods()
    {
        Dictionary<INamedTypeSymbol, ImmutableArray<IMethodSymbol>> map = [];

        foreach (var item in _mapMethods)
            map.Add(item.Key, item.Value.ToImmutableArray());

        var returnMap = map.ToImmutableDictionary(default);
        map.Clear();
        return returnMap;
    }

    public bool Clear()
    {
        foreach (var item in _mapMethods)
            item.Value.Clear();

        _mapMethods.Clear();
        _mapMethods = null!;
        return true;
    }
}
