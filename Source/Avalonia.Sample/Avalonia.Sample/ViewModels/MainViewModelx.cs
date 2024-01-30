using ReactiveUI;

namespace Avalonia.Sample.ViewModels;

[RxObject]
internal partial class MainViewModelx
{
    public MainViewModelx()
    {
        //TestCommand
    }

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