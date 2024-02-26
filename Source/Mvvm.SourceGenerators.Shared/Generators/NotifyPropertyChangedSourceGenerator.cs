using MicroSugar.Mvvm;
using Mvvm.SourceGenerators.Builder;
using SourceGeneratorToolkit.Builders;
using SourceGeneratorToolkit.Diagnostics;
using SourceGeneratorToolkit.Extensions;
using SourceGeneratorToolkit.SyntaxContexts;
using static Mvvm.SourceGenerators.Helpers.CodeHelpers;
using static SourceGeneratorToolkit.Helpers.CommonHelpers;

namespace Mvvm.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class NotifyPropertyChangedSourceGenerator : ISourceGenerator, ICodeXProvider
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(context => context.CreateSourceCodeFromEmbeddedResource(nameof(NotifyPropertyChangedAttribute), __GeneratorCSharpFileHeader__));
        context.RegisterForSyntaxNotifications(() => new ObjectSyntaxContextReceiver(__NotifyPropertyChangedFullAttribute__));
    }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxContextReceiver;
        if (receiver is not ObjectSyntaxContextReceiver syntaxContextReceiver)
            return;

        //Debugger.Launch();
        var map = syntaxContextReceiver.GetClasses();
        foreach (var classSymbol in map)
        {
            if (classSymbol is null)
                continue;

            //Debugger.Launch();
            using CodeBuilderX builder = CodeBuilderX.CreateBuilderX(classSymbol.ContainingNamespace.ToDisplayString(), classSymbol.Name, classSymbol.IsAbstract, this);
            builder.AppendUseSystemNameSpace();

            if (!classSymbol.IsImplementedOf<INotifyPropertyChanged>())
                builder.AppendInterfaceX<INotifyPropertyChanged>();

            if (!classSymbol.IsImplementedOf<INotifyPropertyChanging>())
                builder.AppendInterfaceX<INotifyPropertyChanging>();

            context.AddSource($"{classSymbol.Name}_{nameof(INotifyPropertyChanged)}.{__GeneratorCSharpFileExtension__}", SourceText.From(builder.Build()!, Encoding.UTF8));
        }

        map.Clear();
        syntaxContextReceiver.Clear();
    }

    string ICodeProvider.CreateCommandString(string? argumentType, string? returnType, string methodName, string? canMethodName)
    {
        throw new NotImplementedException();
    }

    string ICodeProvider.GetRaisePropertyString(string fieldName, string propertyName)
    {
        throw new NotImplementedException();
    }

    string ICodeXProvider.CreateMethodString(string? argumentType, string? returnType, string methodName)
    {
        //Debugger.Launch();


        return default!;
    }

    string ICodeXProvider.CreateIocString(InjectType type, string containerName, string? token, string from, string to)
    {
        throw new NotImplementedException();
    }

    string ICodeXProvider.CreateManyIocString(InjectType type, string containerName, string? token, string to, params string[] froms)
    {
        throw new NotImplementedException();
    }

    string ICodeXProvider.CreateNavigationString(string containerName, string? token, string view, string viewModel)
    {
        throw new NotImplementedException();
    }

    string ICodeProvider.CreateClassBodyString()
    {
        //Debugger.Launch();
        return
            $$"""
                public event PropertyChangingEventHandler? PropertyChanging;
                public event PropertyChangedEventHandler? PropertyChanged;

                protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
                {
                    if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

                    RaisePropertyChanging(propertyName);
                    storage = value;
                    RaisePropertyChanged(propertyName);

                    return true;
                }

                protected void RaisePropertyChanging([CallerMemberName] string? propertyName = null)
                {
                    OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
                }

                protected virtual void OnPropertyChanging(PropertyChangingEventArgs args)
                {
                    PropertyChanging?.Invoke(this, args);
                }

                protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
                }

                protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
                {
                    PropertyChanged?.Invoke(this, args);
                }

            """;
    }
}
