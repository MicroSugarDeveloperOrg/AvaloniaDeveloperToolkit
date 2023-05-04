using Avalonia.Controls;
using AvaloniaPropertySourceGenerator.Attributes;

namespace Avalonia.Sample.Views;

[StyledProperty("MyTest", typeof(bool))]
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}