using Prism.Commands;
using Prism.Mvvm;
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
    //    set
    //    {
    //        if (EqualityComparer<string>.Default.Equals(_title, value)) return;

    //        var @old = _title;
    //        var @new = value;

    //        TitleChanging(@old);
    //        TitleChanging(@old, @new);

    //        RaisePropertyChanged();


    //        TitleChanged(@old, @new);
    //        TitleChanged(@new);
    //    }
    //}




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
