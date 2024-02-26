namespace SourceGeneratorToolkit.Builders;
internal class CodeBuilderR : CodeBuilder
{
    protected CodeBuilderR(string nameSpace, string className, bool isAbstract, ICodeProvider? provider)
        : base(nameSpace, className, isAbstract, provider)
    {
    }

    public static CodeBuilderR CreateBuilderX(string nameSpace, string className, bool isAbstract, ICodeXProvider? provider)
       => new CodeBuilderR(nameSpace, className, isAbstract, provider);


    public override string? Build()
    {
        if (string.IsNullOrWhiteSpace(_className))
            return default;
 

        return base.Build();
    }

}
