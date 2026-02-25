using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Windowing;
using ImmersingPicker.Views;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Services.Picker;

namespace ImmersingPicker;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 为每个Clazz创建对应的Picker实例
        foreach (var clazz in Clazz.Classes)
        {
            new FairStudentPicker(clazz);
            new PlainStudentPicker(clazz);
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}