using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Sample.ViewModels;

[ObservableObject]
public partial class TestMvvm 
{

    //[ObservableProperty]
    //public int Test;

    [RelayCommand()]
    void Test(object obj)
    {

    }
}
