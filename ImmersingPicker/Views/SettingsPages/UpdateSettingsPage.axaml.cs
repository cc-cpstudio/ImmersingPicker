using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using ImmersingPicker.Controls;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services;
using ImmersingPicker.Services.Helper;
using ImmersingPicker.Services.Services;
using Serilog;

namespace ImmersingPicker.Views.SettingsPages;

public partial class UpdateSettingsPage : SettingsPageBase
{
    private static readonly ILogger _logger = Log.ForContext<UpdateSettingsPage>();

    public UpdateSettingsPage()
    {
        InitializeComponent();
        PageTitle.Text = "更新管理";
        LoadSettings();
    }

    protected override void LoadSettings()
    {
        // 加载更新设置
        AutoCheckUpdateToggle.IsChecked = AppSettings.AutoCheckUpdateEnabled;
        AllowPrereleaseToggle.IsChecked = AppSettings.AllowPrereleaseUpdates;

        // 显示当前版本
        var currentVersion = VersionHelper.GetCurrentVersion();
        if (currentVersion != null)
        {
            CurrentVersionText.Text = VersionHelper.FormatVersion(currentVersion);
        }
        else
        {
            CurrentVersionText.Text = "未知";
        }
    }

    /// <summary>
    /// 检查更新按钮点击
    /// </summary>
    private async void CheckUpdateButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await CheckForUpdatesManuallyAsync();
    }

    /// <summary>
    /// 手动检查更新
    /// </summary>
    public async Task CheckForUpdatesManuallyAsync()
    {
        try
        {
            CheckUpdateButton.IsEnabled = false;
            CheckUpdateButton.Content = "检查中...";

            _logger.Information("手动检查更新...");
            
            // 检查 SparkleUpdater 是否初始化
            if (!SparkleUpdaterService.Instance.IsInitialized)
            {
                await ShowErrorDialogAsync(
                    "更新服务未初始化", 
                    "自动更新服务未能成功初始化，请检查应用日志获取更多详细信息。");
                return;
            }

            // 使用 NetSparkleUpdater 检查更新
            SparkleUpdaterService.Instance.CheckForUpdatesAtUserRequest();
            
            // 更新最后检查时间
            AppSettings.LastUpdateCheckTime = DateTime.Now;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "检查更新时发生错误");
            await ShowErrorDialogAsync(
                "错误", 
                $"检查更新失败:\n{ex.Message}\n\n详细信息请查看日志文件。");
        }
        finally
        {
            CheckUpdateButton.IsEnabled = true;
            CheckUpdateButton.Content = "检查更新";
        }
    }

    /// <summary>
    /// 显示信息对话框
    /// </summary>
    private async Task ShowInfoDialogAsync(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "确定"
        };

        await dialog.ShowAsync();
    }

    /// <summary>
    /// 显示错误对话框
    /// </summary>
    private async Task ShowErrorDialogAsync(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "确定"
        };

        await dialog.ShowAsync();
    }

    /// <summary>
    /// 自动检查更新开关
    /// </summary>
    private void AutoCheckUpdateToggle_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (AutoCheckUpdateToggle is ToggleSwitch toggle)
        {
            AppSettings.AutoCheckUpdateEnabled = toggle.IsChecked ?? true;
            _logger.Information("自动检查更新: {Enabled}", AppSettings.AutoCheckUpdateEnabled);
        }
    }

    /// <summary>
    /// 允许预发布版本开关
    /// </summary>
    private void AllowPrereleaseToggle_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (AllowPrereleaseToggle is ToggleSwitch toggle)
        {
            AppSettings.AllowPrereleaseUpdates = toggle.IsChecked ?? false;
            _logger.Information("允许预发布版本: {Enabled}", AppSettings.AllowPrereleaseUpdates);
        }
    }

    /// <summary>
    /// 更新说明按钮点击
    /// </summary>
    private async void UpdateInfoButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "更新说明",
            Content = new ScrollViewer
            {
                MaxHeight = 400,
                Content = new TextBlock
                {
                    Text = "• 现在使用 NetSparkleUpdater 进行自动更新\n" +
                           "• 更新会自动下载并安装\n" +
                           "• 安装完成后应用会自动重启",
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 24,
                    FontSize = 14
                }
            },
            CloseButtonText = "确定"
        };

        await dialog.ShowAsync();
    }
}
