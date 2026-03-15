using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Interactivity;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using ImmersingPicker.Services;
using Serilog;

namespace ImmersingPicker.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        Log.Information("主窗口初始化开始");
        Log.Verbose("开始初始化 MainWindow 组件");
        InitializeComponent();

        Log.Verbose("设置 TitleBar 属性");
        TitleBar.Height = 36;
        TitleBar.ExtendsContentIntoTitleBar = false;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;

        Log.Verbose("初始化导航服务");
        MainWindowNavigationService.Initialize(ContentFrame, this);
        Log.Information("主窗口导航服务初始化完成");
        
        // 添加关闭事件处理
        Log.Verbose("添加窗口关闭事件处理");
        Closing += MainWindow_Closing;
        Log.Information("主窗口初始化完成");
    }

    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        MainNavView.SelectedItem = HomePageItem;
    }

    private void MainNavView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs? e)
    {
        if (e is not { SelectedItem: NavigationViewItem { Tag: string tag } }) return;
        var viewType = tag switch
        {
            "Home" => MainWindowNavigationService.ViewType.Home,
            "History" => MainWindowNavigationService.ViewType.History,
            "Editor" => MainWindowNavigationService.ViewType.Editor,
            "Settings" => MainWindowNavigationService.ViewType.Settings,
            _ => throw new ArgumentException("Invalid view type")
        };
        MainWindowNavigationService.NavigateTo(viewType);
        if (viewType is MainWindowNavigationService.ViewType.Settings or MainWindowNavigationService.ViewType.Editor)
        {
            MainNavView.SelectedItem = HomePageItem;
        }
    }
    
    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        Log.Information("主窗口关闭事件触发");
        // 取消关闭操作
        e.Cancel = true;
        Log.Warning("窗口关闭操作被取消，改为隐藏窗口");
        // 隐藏窗口
        Hide();
        Log.Information("主窗口已隐藏");
        
        // 保存数据
        try
        {
            Log.Information("开始保存班级数据");
            var storageService = new ImmersingPicker.Services.Services.Storage.ClassStorageService();
            int classCount = ImmersingPicker.Core.Models.Clazz.Classes.Count;
            Log.Verbose("班级数量: {ClassCount}", classCount);
            storageService.SaveClasses(ImmersingPicker.Core.Models.Clazz.Classes);
            Log.Information("班级数据保存完成");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "保存班级数据失败");
        }
    }
}