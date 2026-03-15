using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using ImmersingPicker.Controls;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Services;

namespace ImmersingPicker.Views.SettingsPages;

public partial class SecurityAndPrivacySettingsPage : UserControl
{
    private readonly PasswordService _passwordService = PasswordService.Instance;

    public SecurityAndPrivacySettingsPage()
    {
        InitializeComponent();
        LoadSettings();
        CheckPasswordGroupEnabled();
    }

    private void LoadSettings()
    {
        OpenPassword.IsChecked = AppSettings.Instance.OpenPassword;
    }

    private void CheckPasswordGroupEnabled()
    {
        SetPasswordItem.IsEnabled = OpenPassword.IsChecked ?? false;
    }

    private void OpenPassword_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        AppSettings.Instance.OpenPassword = OpenPassword.IsChecked ?? false;
        CheckPasswordGroupEnabled();
    }

    private async void SetPassword_OnClick(object? sender, RoutedEventArgs e)
    {
        await ShowPasswordDialog();
    }

    private async Task ShowPasswordDialog()
    {
        var passwordDialog = new PasswordDialog();
        passwordDialog.Clear();
        passwordDialog.SetMode(_passwordService.HasPassword);

        var parentWindow = TopLevel.GetTopLevel(this) as Window;
        if (parentWindow == null) return;

        var dialog = new ContentDialog
        {
            Title = _passwordService.HasPassword ? "修改密码" : "设置密码",
            Content = passwordDialog,
            PrimaryButtonText = "确认",
            CloseButtonText = "取消",
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await dialog.ShowAsync(parentWindow);

        if (result == ContentDialogResult.Primary)
        {
            if (passwordDialog.Validate())
            {
                if (passwordDialog.SavePassword())
                {
                    var successDialog = new ContentDialog
                    {
                        Title = "成功",
                        Content = _passwordService.HasPassword ? "密码修改成功" : "密码设置成功",
                        CloseButtonText = "确定"
                    };
                    await successDialog.ShowAsync(parentWindow);
                }
                else
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "失败",
                        Content = "密码设置失败，请重试",
                        CloseButtonText = "确定"
                    };
                    await errorDialog.ShowAsync(parentWindow);
                }
            }
            else
            {
                await ShowPasswordDialog();
            }
        }
    }
}