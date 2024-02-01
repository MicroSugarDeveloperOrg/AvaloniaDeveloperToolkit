using SourceGeneratorToolkit.Extensions;

namespace SourceGeneratorToolkit.SyntaxContexts;

internal sealed class RecordRegistrarClass(INamedTypeSymbol classSymbol, IMethodSymbol registerMethodSymbol)
{
    public INamedTypeSymbol ClassSymbol { get; } = classSymbol;
    public IMethodSymbol RegisterMethodSymbol { get; } = registerMethodSymbol;
}

internal class RegistrarSyntaxContextReceiver : ISyntaxContextReceiver
{
    public RegistrarSyntaxContextReceiver(string registrarAttribute, params string[] injectStrings)
    {
        _registrarAttribute = registrarAttribute;
        _injectStrings = injectStrings.ToImmutableArray();
    }

    string _registrarAttribute;
    ImmutableArray<string> _injectStrings;

    RecordRegistrarClass? _registrarClass;
    Dictionary<INamedTypeSymbol, AttributeData> _mapAttributeDatas = [];

    void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration) return;

        if (classDeclaration.AttributeLists.Count <= 0) return;

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
        if (classSymbol is null) return;
         
        foreach (var attributeListSyntax in classDeclaration.AttributeLists)
        {
            if (attributeListSyntax is null)  continue;
            if (attributeListSyntax.Attributes.Count <= 0)  continue;

            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (attributeSyntax is null) continue;

                var namedTypeSymbol = context.SemanticModel.GetTypeInfo(attributeSyntax).Type as INamedTypeSymbol;
                if (namedTypeSymbol is null) continue;

                classSymbol.TryGetAttributeWithType(namedTypeSymbol, out var attributeData);
                if (attributeData is null) continue;

                if (namedTypeSymbol.MetadataName == _registrarAttribute)
                {
                    if (_registrarClass is not null) continue;

                    var arguments = attributeData.ConstructorArguments.FirstOrDefault();
                    var argumentValue = arguments.Value?.ToString();
                    if (string.IsNullOrWhiteSpace(argumentValue)) continue;

                    var registerMethodSymbol = classSymbol.GetMembers(argumentValue!).FirstOrDefault() as IMethodSymbol;
                    if (registerMethodSymbol is null) continue;

                    _registrarClass = new(classSymbol, registerMethodSymbol);
                    break;
                }

                if (_injectStrings.Contains(namedTypeSymbol.MetadataName))
                {
                    _mapAttributeDatas[classSymbol] = attributeData;
                    break;
                }
            }
        }   
    }

    public (RecordRegistrarClass? registrarClass, ImmutableDictionary<INamedTypeSymbol, AttributeData> mapAttributeDatas) GetRegistrations()
    {
        return (_registrarClass, _mapAttributeDatas.ToImmutableDictionary(SymbolEqualityComparer.Default));
    }

    public bool Clear()
    {
        _registrarClass = null;
        _mapAttributeDatas.Clear();
        _mapAttributeDatas = default!;
        return true;
    }

}
