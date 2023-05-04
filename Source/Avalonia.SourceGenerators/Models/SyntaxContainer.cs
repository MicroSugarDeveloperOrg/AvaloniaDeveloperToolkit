namespace AvaloniaPropertySourceGenerator.Models;
internal sealed record SyntaxContainer<TValue>(TValue Value) where TValue : IEquatable<TValue>?;