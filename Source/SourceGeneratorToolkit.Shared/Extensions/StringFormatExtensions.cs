namespace SourceGeneratorToolkit.Extensions;

static class StringFormatExtensions
{
    public static string FirstCharToLow(this string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return word;

        return $"{char.ToLower(word[0])}{word.Substring(1)}";
    }
}
