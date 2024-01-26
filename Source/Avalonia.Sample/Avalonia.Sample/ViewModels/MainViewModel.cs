using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Commands;
using Prism.Mvvm;
using System.Configuration;
using System.Windows.Input;

namespace Avalonia.Sample.ViewModels;

[BindableObject]
public partial class MainViewModel
{
    public string Greeting => "Welcome to Avalonia!";

    [BindableProperty]
    string _title = string.Empty;

    //[BindableProperty]
    //public string Txt = string.Empty;

    //public string Title
    //{
    //    get => _title;
    //    set => SetProperty(ref _title, value, (@old, @new) =>
    //    {
    //        TitleChanging(@old);
    //        TitleChanging(@old, @new);
    //    }, (@old, @new) =>
    //    {
    //        TitleChanged(@old, @new);
    //        TitleChanged(@new);
    //    });
    //}

    //partial void TitleChanging(string oldValue);
    //partial void TitleChanging(string oldValue, string newValue);
    //partial void TitleChanged(string oldValue, string newValue);
    //partial void TitleChanged(string newValue);



    //DelegateCommand<object>? _command;

    //public ICommand Command => _command ??= new DelegateCommand<object>(Test, CanTest);

    [BindableCommand(nameof(CanTest))]
    void Test(object obj)
    {
         
    }

    bool CanTest(object obj)
    {
        return true;
    }

}
