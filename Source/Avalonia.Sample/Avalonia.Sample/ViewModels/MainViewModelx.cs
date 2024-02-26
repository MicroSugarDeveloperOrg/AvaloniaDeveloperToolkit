using ReactiveUI;
using System.ComponentModel.DataAnnotations;

namespace Avalonia.Sample.ViewModels;

[RxObject]
internal partial class MainViewModelx
{
    public MainViewModelx()
    {
        //TestCommand
    }

    [property: Key]
    [RxProperty]
    string? _title;

    [RxCommand(nameof(CanTest))]
    bool Test(string obj)
    {
        return true;
    }

    bool CanTest()
    {
        return true;
    }
}