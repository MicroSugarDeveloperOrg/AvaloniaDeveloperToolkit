using Prism.Commands;
using Prism.Mvvm;

namespace Avalonia.Sample.ViewModels;

[BindableObject]
public partial class MainViewModel
{
    public MainViewModel()
    {
        
    }

    [BindableProperty]
    string _greeting = "\"Welcome to Avalonia!\"";

    partial void GreetingChanging(string oldValue, string newValue)
    {
    }

    partial void GreetingChanged(string oldValue, string newValue)
    {
    }

    [BindableCommand(nameof(CanTest))]
    void Test(object obj)
    {

    }

    bool CanTest(object obj)
    {
        return true;
    }

}
