namespace SourceGeneratorToolkit.SyntaxContexts;

public record AttributeForProperty(INamedTypeSymbol Symbol, string AttributeString)
{ }


internal sealed class PropertySyntaxContextReceiver : ISyntaxContextReceiver
{
    public PropertySyntaxContextReceiver(string propertyAttributeString)
    {
        PropertyAttributeString = propertyAttributeString;
    }

    Dictionary<INamedTypeSymbol, List<IFieldSymbol>> _mapFields = [];
    Dictionary<IFieldSymbol, List<AttributeForProperty>> _mapPropertyAttributes = [];

    public string PropertyAttributeString { get; init; }

    void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax && fieldDeclarationSyntax.AttributeLists.Count > 0)
        {
            foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
            {
                var symbol = context.SemanticModel.GetDeclaredSymbol(variable);
                if (symbol is not IFieldSymbol fieldSymbol)
                    continue;

                _mapPropertyAttributes.TryGetValue(fieldSymbol, out var attributesProperty);
                if (attributesProperty is null)
                {
                    attributesProperty = [];
                    _mapPropertyAttributes[fieldSymbol] = attributesProperty;
                }

                foreach (var attributeListSyntax in fieldDeclarationSyntax.AttributeLists)
                {
                    if (attributeListSyntax is null) continue;
                    if (attributeListSyntax.Attributes.Count <= 0) continue;

                    if (attributeListSyntax.Target?.Identifier.RawKind != (int)SyntaxKind.PropertyKeyword) continue;

                    foreach (var attributeSyntax in attributeListSyntax.Attributes)
                    {
                        if (attributeSyntax is null) continue;
                        var namedTypeSymbol = context.SemanticModel.GetTypeInfo(attributeSyntax).Type as INamedTypeSymbol;
                        if (namedTypeSymbol is null) continue;

                        var asString = attributeSyntax.ToString();

                        attributesProperty.Add(new AttributeForProperty(namedTypeSymbol, asString));
                    }
                }

                var attributes = fieldSymbol.GetAttributes();
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

    public ImmutableArray<AttributeForProperty>? GetPropertyAttributes(IFieldSymbol symbol)
    {
        _mapPropertyAttributes.TryGetValue(symbol, out var attributes);
        if (attributes is null)
            return default;

        return attributes.ToImmutableArray();
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
