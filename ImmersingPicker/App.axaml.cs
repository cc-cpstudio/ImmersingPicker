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
using ImmersingPicker.Services.Services;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Controls.Primitives;
using Avalonia.Platform.Storage;
using ImmersingPicker.Services;
using ImmersingPicker.Helpers;
using Serilog;

namespace ImmersingPicker;

public partial class App : Application
{
    private Timer? _autoSaveTimer;
    private MainWindow? _mainWindow;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Log.Information("应用程序框架初始化完成，开始加载数据");
        // 加载班级数据
        try
        {
            Log.Information("开始加载班级数据");
            Log.Verbose("获取ClassStorageService实例");
            var storageService = ClassStorageService.Instance;
            storageService.LoadClasses();
            Log.Information("班级数据加载完成");
            Log.Verbose("班级数量: {Count}", Clazz.Classes.Count);
        }
        catch (Exception ex)
        {
            // 如果加载失败，使用默认数据
            Log.Error(ex, "加载班级数据失败");
            Log.Warning("使用默认班级数据");
        }

        try
        {
            Log.Information("开始加载应用设置");
            Log.Verbose("获取SettingsStorageService实例");
            var storageService = SettingsStorageService.Instance;
            storageService.LoadSettings();
            Log.Information("应用设置加载完成");
            Log.Verbose("当前主题: {Theme}", AppSettings.Instance.AppTheme);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "加载应用设置失败");
            Log.Warning("使用默认应用设置");
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

        // 初始化平台服务
        var platformServices = PlatformServices.Instance;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow = new MainWindow();
            desktop.MainWindow = _mainWindow;
        }

        var a = AppSettings.Instance.AppTheme;

        // 初始化主题管理器
        Log.Information("初始化主题管理器");
        ThemeManager.Instance.Initialize();
        Log.Information("主题管理器初始化完成");

        // 初始化自动保存定时器
        InitializeAutoSaveTimer();

        base.OnFrameworkInitializationCompleted();
    }

    private void InitializeAutoSaveTimer()
    {
        Log.Information("初始化自动保存定时器");
        Log.Verbose("设置定时器间隔为300秒");
        _autoSaveTimer = new Timer(300000); // 300秒
        Log.Verbose("添加定时器事件处理");
        _autoSaveTimer.Elapsed += AutoSaveTimer_Elapsed;
        Log.Verbose("设置定时器自动重置");
        _autoSaveTimer.AutoReset = true;
        Log.Verbose("启动定时器");
        _autoSaveTimer.Start();
        Log.Information("自动保存定时器初始化完成");
    }

    private void AutoSaveTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            Log.Information("开始自动保存班级数据");
            var storageService = ClassStorageService.Instance;
            storageService.SaveClasses(Clazz.Classes);
            Log.Information("班级数据自动保存完成");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "自动保存班级数据失败");
        }

        try
        {
            Log.Information("开始自动保存应用设置");
            var storageService = SettingsStorageService.Instance;
            storageService.SaveSettings();
            Log.Information("应用设置自动保存完成");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "自动保存应用设置失败");
        }
    }

    // 应用程序关闭时的清理操作
    public void Shutdown()
    {
        Log.Information("开始应用程序关闭清理操作");
        
        // 停止定时器
        Log.Information("停止自动保存定时器");
        try
        {
            _autoSaveTimer?.Stop();
            _autoSaveTimer?.Dispose();
            Log.Verbose("定时器已停止并释放");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "停止定时器失败");
        }

        // 退出时保存数据
        try
        {
            Log.Information("开始保存班级数据");
            var storageService = ClassStorageService.Instance;
            storageService.SaveClasses(Clazz.Classes);
            Log.Information("班级数据保存完成");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "保存班级数据失败，可能导致数据丢失");
        }

        try
        {
            Log.Information("开始保存应用设置");
            var storageService = SettingsStorageService.Instance;
            storageService.SaveSettings();
            Log.Information("应用设置保存完成");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "保存应用设置失败，可能导致设置丢失");
        }
        
        Log.Information("应用程序关闭清理操作完成");
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