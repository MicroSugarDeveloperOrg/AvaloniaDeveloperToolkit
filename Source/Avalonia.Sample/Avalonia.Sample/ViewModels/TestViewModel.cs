using MicroSugar.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avalonia.Sample.ViewModels;

[NotifyPropertyChanged]
internal partial class TestViewModel
{
    [property: Key]
    [property: Column("id")]
    [AutoProperty]
    string? _id;

    [property: Key]
    [property: Column("test_id")]
    [AutoProperty]
    string? _testId;
}
