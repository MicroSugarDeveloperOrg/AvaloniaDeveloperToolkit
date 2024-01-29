namespace SourceGeneratorToolkit.Builders;

internal interface ICodeProvider
{
    string GetRaisePropertyString(string fieldName, string propertyName);
    string CreateCommandString(string? argumentType, string? returnType, string methodName, string? canMethodName);
}
