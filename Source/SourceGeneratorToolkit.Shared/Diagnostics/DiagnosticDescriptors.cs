using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SourceGeneratorToolkit.Diagnostics;
internal class DiagnosticDescriptors
{
    public static DiagnosticDescriptor CreateDuplicateINotifyPropertyChangedInterfaceForBindableObjectAttributeError<TSourceGenerator>(string objectName) where TSourceGenerator : ISourceGenerator
    {
        return new DiagnosticDescriptor(
                   id: "MVVMSG0002",
                   title: $"Duplicate {nameof(INotifyPropertyChanged)} definition",
                   messageFormat: $"Cannot apply [{objectName}] to type {{0}}, as it already declares the {nameof(INotifyPropertyChanged)} interface",
                   category: typeof(TSourceGenerator).FullName,
                   defaultSeverity: DiagnosticSeverity.Error,
                   isEnabledByDefault: true,
                   description: $"Cannot apply [{objectName}] to a type that already declares the {nameof(INotifyPropertyChanged)} interface.",
                   helpLinkUri: "");
    }

 
}
