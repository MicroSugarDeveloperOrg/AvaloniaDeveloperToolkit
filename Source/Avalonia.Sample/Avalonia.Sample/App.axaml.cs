using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Sample.Service;
using Avalonia.Sample.ViewModels;
using Avalonia.Sample.Views;
using Prism.DryIoc;
using Prism.Ioc;

namespace Avalonia.Sample;

[Registrar(nameof(RegisterServices))]
public partial class App : PrismApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();  
    }

    protected override AvaloniaObject CreateShell()
    {
        AvaloniaObject shell = default!;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            shell = new MainWindow
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
        RegisterServices(containerRegistry);
        RegisterViewViewModel(containerRegistry);
        RegisterForNavigation(containerRegistry);
    }

    [Singleton(typeof(ITestService), typeof(TestService))]
    [Singleton(typeof(ITestService), typeof(TestService), Token = "Test1")]
    [Singleton<TestService>]
    [Singleton<ITestService, TestService>]
    [Singleton<ITestService, TestService>(Token = "Test3")]

    [Scoped(typeof(ITestService), typeof(TestService))]
    [Scoped(typeof(ITestService), typeof(TestService), Token = "Test1")]
    [Scoped<TestService>]
    [Scoped<ITestService, TestService>]
    [Scoped<ITestService, TestService>(Token = "Test3")]

    [Transient(typeof(ITestService), typeof(TestService))]
    [Transient(typeof(ITestService), typeof(TestService), Token = "Test1")]
    [Transient<TestService>]
    [Transient<ITestService, TestService>]
    [Transient<ITestService, TestService>(Token = "Test3")]

    [ManySingleton<TestService>(typeof(ITestService), typeof(ITestService2), Token = "Test5")]
    [ManyScoped<TestService>(typeof(ITestService), typeof(ITestService2), Token = "Test6")]
    [ManyTransient<TestService>(typeof(ITestService), typeof(ITestService2), Token = "Test7")]
    partial void RegisterViewViewModel(IContainerRegistry containerRegistry);

    [Navigation<MainWindow>(Token = nameof(MainWindow))]
    [Navigation<MainWindow, MainViewModel>(Token = nameof(MainWindow))]
    [Navigation<MainView, MainViewModel>(Token = nameof(MainView))]
    partial void RegisterForNavigation(IContainerRegistry containerRegistry);

    partial void RegisterServices(IContainerRegistry containerRegistry);

}