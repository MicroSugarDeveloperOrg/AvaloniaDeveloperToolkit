namespace SourceGeneratorToolkit.Builders;

internal interface ICodeXProvider : ICodeProvider
{
    string CreateIocString(InjectType type, string containerName, string? token, string from, string to);
    string CreateManyIocString(InjectType type, string containerName, string? token, string to, params string[] froms);
    string CreateNavigationString(string containerName, string? token, string view, string viewModel);
}
