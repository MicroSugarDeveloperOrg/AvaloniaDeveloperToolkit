namespace SourceGeneratorToolkit.SyntaxContexts;

internal sealed class ObjectSyntaxContextReceiver : ISyntaxContextReceiver
{
    public ObjectSyntaxContextReceiver(string objectAttributeString)
    {
        ObjectAttributeString = objectAttributeString;
    }

    List<INamedTypeSymbol> _mapClasses = [];

    public string ObjectAttributeString { get; init; } 

    void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration)
            return;

        if (classDeclaration.AttributeLists.Count <= 0)
            return;

        bool isFlag = false;
        foreach (var item in classDeclaration.AttributeLists)
        {
            if (item is null)
                continue;

            if (item.Attributes.Count <= 0)
                continue;

            foreach (var attributeSyntax in item.Attributes)
            {
                var type = context.SemanticModel.GetTypeInfo(attributeSyntax).Type;
                if (type is null)
                    continue;

                if (type.ToDisplayString() != ObjectAttributeString)
                    continue;

                isFlag = true;
                break;
            }

            if (isFlag)
                break;
        }

        if (isFlag)
        {
            var namedTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
            if (namedTypeSymbol is null)
                return;

            _mapClasses.Add(namedTypeSymbol);
        }
    }

    public ImmutableArray<INamedTypeSymbol> GetClasses() => _mapClasses.ToImmutableArray();

    public bool Clear()
    {
        _mapClasses.Clear();
        _mapClasses = null!;
        return true;
    }
}
