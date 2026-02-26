using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Windowing;
using ImmersingPicker.Views;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Services.Picker;
using ImmersingPicker.Services.Services.Storage;

namespace ImmersingPicker;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 加载班级数据
        try
        {
            var storageService = new ClassStorageService();
            storageService.LoadClasses();
        }
        catch (Exception ex)
        {
            // 如果加载失败，使用默认数据
            // 这里可以添加日志记录
        }

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