using Avalonia.Controls;
using AvaloniaPropertySourceGenerator.Attributes;

namespace Avalonia.Sample.Views;

[StyledProperty("MyTest",typeof(bool))]
public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }
}