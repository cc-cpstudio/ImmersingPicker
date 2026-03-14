using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Serilog;

namespace ImmersingPicker.Services;

public class WelcomeWindowNavigationService
{
    private static Frame? _mainContentFrame;

    public static void Initialize(Frame mainFrame)
    {
        Log.Information("初始化欢迎窗口导航服务");
        Log.Verbose("设置主内容框架");
        _mainContentFrame = mainFrame;
        Log.Information("设置窗口导航服务初始化完成");
    }

    public static void NavigateTo(Type viewType)
    {
        Log.Information("导航到欢迎视图: {ViewType}", viewType.Name);
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

            }
        }
        else
        {
            Log.Error("主内容框架未初始化");
        }
    }

    public static void NavigateTo(ViewType viewType)
    {
        Log.Information("导航到欢迎视图类型: {ViewType}", viewType);
        try
        {
            Type targetType = viewType switch
            {
                ViewType.Welcome => typeof(Views.WelcomePages.WelcomePage),
                ViewType.License => typeof(Views.WelcomePages.LicensePage),
                ViewType.ClazzInitialization => typeof(Views.WelcomePages.ClazzInitializationPage),
                ViewType.Appearance => typeof(Views.WelcomePages.AppearancePage),
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
        Welcome, License, ClazzInitialization, Appearance
    }
}