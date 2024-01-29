using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Avalonia.Sample.ViewModels;

[RxObject]
internal partial class MainViewModelx
{
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