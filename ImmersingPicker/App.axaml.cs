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
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Controls.Primitives;
using Avalonia.Platform.Storage;
using ImmersingPicker.Services;

namespace ImmersingPicker;

public partial class App : Application
{
    private Timer? _autoSaveTimer;
    private MainWindow? _mainWindow;
    private SuspendingWindow? _suspendingWindow;

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
            Console.WriteLine(ex);
        }

        // 为每个Clazz创建对应的Picker实例（如果还没有的话）
        foreach (var clazz in Clazz.Classes)
        {
            if (!clazz.Pickers.ContainsKey("FairStudentPicker"))
            {
                new FairStudentPicker(clazz);
            }
            if (!clazz.Pickers.ContainsKey("PlainStudentPicker"))
            {
                new PlainStudentPicker(clazz);
            }
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow = new MainWindow();
            desktop.MainWindow = _mainWindow;
            
            _suspendingWindow = new SuspendingWindow();
            _suspendingWindow.Show();
        }

        // 初始化自动保存定时器
        InitializeAutoSaveTimer();

        base.OnFrameworkInitializationCompleted();
    }

    private void InitializeAutoSaveTimer()
    {
        _autoSaveTimer = new Timer(300000); // 300秒
        _autoSaveTimer.Elapsed += AutoSaveTimer_Elapsed;
        _autoSaveTimer.AutoReset = true;
        _autoSaveTimer.Start();
    }

    private void AutoSaveTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            var storageService = new ClassStorageService();
            storageService.SaveClasses(Clazz.Classes);
        }
        catch (Exception)
        {
            // 这里可以添加日志记录
        }
    }

    // 应用程序关闭时的清理操作
    public void Shutdown()
    {
        // 停止定时器
        _autoSaveTimer?.Stop();
        _autoSaveTimer?.Dispose();

        // 退出时保存数据
        try
        {
            var storageService = new ClassStorageService();
            storageService.SaveClasses(Clazz.Classes);
        }
        catch (Exception)
        {
            // 这里可以添加日志记录
        }
    }

    // 托盘图标事件处理方法
    public void ShowMainWindow(object? sender, EventArgs e)
    {
        if (_mainWindow != null)
        {
            _mainWindow.Show();
            _mainWindow.Activate();
            _mainWindow.Focus();
        }
    }

    private void OpenEditor(object? sender, EventArgs e)
    {
        // 打开编辑器窗口
        MainWindowNavigationService.OpenEditorWindow();
    }

    private void OpenSettings(object? sender, EventArgs e)
    {
        // 打开设置窗口
        MainWindowNavigationService.OpenSettingsWindow();
    }

    private void RestartApplication(object? sender, EventArgs e)
    {
        // 这里可以实现重新启动应用程序的逻辑
        // 暂时先显示主界面
        ShowMainWindow(sender, e);
    }

    private void ExitApplication(object? sender, EventArgs e)
    {
        // 执行清理操作
        Shutdown();
        
        // 退出应用程序
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}