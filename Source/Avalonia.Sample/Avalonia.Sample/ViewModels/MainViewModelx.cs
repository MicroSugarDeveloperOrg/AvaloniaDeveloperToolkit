using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Avalonia.Sample.ViewModels;
internal class MainViewModelx : ReactiveObject
{
    ReactiveCommand<string,Unit>? _command;

    //public ICommand Command => _command ??= ReactiveCommand.Create<string, Unit>(Test,);

    void Test()
    {

    }


}
