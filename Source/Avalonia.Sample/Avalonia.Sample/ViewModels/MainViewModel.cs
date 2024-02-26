using Prism.Commands;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avalonia.Sample.ViewModels;

[BindableObject]
public partial class MainViewModel : TestUserControlViewModel
{
    public MainViewModel()
    {
        
    }

    [property: Column("id")]
    [property: Key]
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

    [BindableCommand]
    void Test1()
    {

    }

}
