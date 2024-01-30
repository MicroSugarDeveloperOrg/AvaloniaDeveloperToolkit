using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Sample.ViewModels;
using Avalonia.Sample.Views;
using Prism.Ioc;

namespace Avalonia.Sample;
public partial class App : Prism.PrismApplicationBase
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override IContainerExtension CreateContainerExtension()
    {
        throw new NotImplementedException();
    }

    protected override AvaloniaObject CreateShell()
    {
        AvaloniaObject shell = default!;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            shell =  new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            shell = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        return shell;
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        RegisterViewViewModel(containerRegistry);
        RegisterForNavigation(containerRegistry);

        //containerRegistry.RegisterSingleton(,);
        //containerRegistry.RegisterScoped
        //containerRegistry.Register
        //containerRegistry.RegisterForNavigation()
    }

    partial void RegisterViewViewModel(IContainerRegistry containerRegistry);

    partial void RegisterForNavigation(IContainerRegistry containerRegistry);

}