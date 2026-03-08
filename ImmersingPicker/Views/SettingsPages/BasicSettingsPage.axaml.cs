using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Views.SettingsPages;

public partial class BasicSettingsPage : UserControl
{
    public BasicSettingsPage()
    {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings()
    {
        // 加载开机自启动设置
        LaunchOnSystemStart.IsChecked = AppSettings.Instance.LaunchOnSystemStart;
        
        // 加载URL协议与IPC服务设置
        OpenUrlAndIpc.IsChecked = AppSettings.Instance.OpenUrlAndIpc;
        
        // 加载语言设置
        Language.SelectedIndex = AppSettings.Instance.Language switch
        {
            AppSettings.LanguageEnums.System => 0,
            AppSettings.LanguageEnums.SimplifiedChinese => 1,
            AppSettings.LanguageEnums.TraditionalChinese => 2,
            AppSettings.LanguageEnums.English => 3,
            _ => 0
        };
        
        // 加载主题设置
        AppTheme.SelectedIndex = AppSettings.Instance.AppTheme switch
        {
            AppSettings.ThemeEnums.System => 2,
            AppSettings.ThemeEnums.Light => 0,
            AppSettings.ThemeEnums.Dark => 1,
            _ => 2
        };
        
        // 加载主题颜色设置
        if (!string.IsNullOrEmpty(AppSettings.Instance.AppThemeColor))
        {
            try
            {
                AppThemeColor.Color = Avalonia.Media.Color.Parse(AppSettings.Instance.AppThemeColor);
            }
            catch {}
        }
    }

    private void LaunchOnSystemStart_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        AppSettings.Instance.LaunchOnSystemStart = LaunchOnSystemStart.IsChecked ?? false;
    }

    private void OpenUrlAndIpc_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        AppSettings.Instance.OpenUrlAndIpc = OpenUrlAndIpc.IsChecked ?? false;
    }

    private void Language_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        AppSettings.Instance.Language = Language.SelectedIndex switch
        {
            0 => AppSettings.LanguageEnums.System,
            1 => AppSettings.LanguageEnums.SimplifiedChinese,
            2 => AppSettings.LanguageEnums.TraditionalChinese,
            3 => AppSettings.LanguageEnums.English,
            _ => AppSettings.LanguageEnums.System
        };
    }

    private void AppTheme_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        AppSettings.Instance.AppTheme = AppTheme.SelectedIndex switch
        {
            0 => AppSettings.ThemeEnums.System,
            1 => AppSettings.ThemeEnums.Light,
            2 => AppSettings.ThemeEnums.Dark,
            _ => AppSettings.ThemeEnums.System
        };
    }

    private void AppThemeColor_OnColorChanged(object? sender, ColorChangedEventArgs e)
    {
        AppSettings.Instance.AppThemeColor = AppThemeColor.Color.ToString();
    }
}