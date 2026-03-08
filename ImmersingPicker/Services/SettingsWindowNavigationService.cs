using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Serilog;

namespace ImmersingPicker.Services;

public class SettingsWindowNavigationService
{
    private static Frame? _mainContentFrame;

    public static void Initialize(Frame mainFrame)
    {
        Log.Information("初始化设置窗口导航服务");
        Log.Verbose("设置主内容框架");
        _mainContentFrame = mainFrame;
        Log.Information("设置窗口导航服务初始化完成");
    }

    public static void NavigateTo(Type viewType)
    {
        Log.Information("导航到设置视图: {ViewType}", viewType.Name);
        if (_mainContentFrame != null)
        {
            try
            {
                Log.Verbose("创建视图实例");
                if (Activator.CreateInstance(viewType) is UserControl view)
                {
                    Log.Verbose("设置视图为框架内容");
                    _mainContentFrame.Content = view;
                    Log.Information("导航成功");
                }
                else
                {
                    Log.Warning("无法创建视图实例: {ViewType}", viewType.Name);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "导航失败");
            }
        }
        else
        {
            Log.Error("主内容框架未初始化");
        }
    }

    public static void NavigateTo(ViewType viewType)
    {
        Log.Information("导航到设置视图类型: {ViewType}", viewType);
        try
        {
            Type targetType = viewType switch
            {
                ViewType.BasicSettings => typeof(Views.SettingsPages.BasicSettingsPage),
                ViewType.PickerSettings => typeof(Views.SettingsPages.PickerSettingsPage),
                ViewType.About => typeof(Views.SettingsPages.AboutPage),
                _ => throw new ArgumentException("Invalid view type")
            };
            Log.Verbose("解析视图类型为: {TargetType}", targetType.Name);
            NavigateTo(targetType);
        }
        catch (ArgumentException ex)
        {
            Log.Error(ex, "无效的视图类型: {ViewType}", viewType);
        }
    }

    public enum ViewType
    {
        BasicSettings,
        PickerSettings,
        About
    }
}