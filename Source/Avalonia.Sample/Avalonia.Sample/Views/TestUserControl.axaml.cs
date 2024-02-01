using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Sample.ViewModels;
using Prism.Ioc;

namespace Avalonia.Sample;

[Navigation<TestUserControl, TestUserControlViewModel>()]
public partial class TestUserControl : UserControl
{
    public TestUserControl()
    {
        InitializeComponent();
    }
}