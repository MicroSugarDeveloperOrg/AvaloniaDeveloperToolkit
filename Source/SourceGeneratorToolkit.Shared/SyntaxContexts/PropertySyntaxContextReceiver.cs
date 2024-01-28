namespace SourceGeneratorToolkit.SyntaxContexts;

internal sealed class PropertySyntaxContextReceiver : ISyntaxContextReceiver
{
    Dictionary<INamedTypeSymbol, List<IFieldSymbol>> _mapFields = [];

    public string PropertyAttributeString { get; set; } = string.Empty;

    void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax && fieldDeclarationSyntax.AttributeLists.Count > 0)
        {
            foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
            {
                var symbol = context.SemanticModel.GetDeclaredSymbol(variable);
                if (symbol is not IFieldSymbol fieldSymbol)
                    continue;

                if (fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass?.ToDisplayString() == PropertyAttributeString))
                {
                    var type = fieldSymbol.ContainingType;
                    var bRet = _mapFields.TryGetValue(type, out var value);
                    if (value is null)
                    {
                        value = new List<IFieldSymbol>();
                        _mapFields.Add(type, value);
                    }

                    value.Add(fieldSymbol);
                }
            }
        }
    }

    public ImmutableDictionary<INamedTypeSymbol, ImmutableArray<IFieldSymbol>> GetFields()
    {
        Dictionary<INamedTypeSymbol, ImmutableArray<IFieldSymbol>> map = [];

        foreach (var item in _mapFields)
            map.Add(item.Key, item.Value.ToImmutableArray());

        var returnMap = map.ToImmutableDictionary(default);
        map.Clear();
        return returnMap;
    }

    public bool Clear()
    {
        foreach (var item in _mapFields)
            item.Value.Clear();

        _mapFields.Clear();
        _mapFields = null!;
        return true;
    }
}
