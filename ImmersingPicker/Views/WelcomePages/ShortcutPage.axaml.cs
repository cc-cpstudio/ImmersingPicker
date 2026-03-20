using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services;
using ImmersingPicker.Services.Services;

namespace ImmersingPicker.Views.WelcomePages;

public partial class ShortcutPage : UserControl
{
    public ShortcutPage()
    {
        InitializeComponent();
        NextButton.Background = Brush.Parse(AppSettings.Instance.AppThemeColor);
    }

    private void NextButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var platformServices = PlatformServices.Instance;

        if (DesktopShortcut.IsChecked == true)
        {
            platformServices.CreateDesktopShortcut();
        }

        if (StartMenuShortcut.IsChecked == true)
        {
            platformServices.CreateStartMenuShortcut();
        }

        WelcomeWindowNavigationService.NavigateTo(WelcomeWindowNavigationService.ViewType.Congratulation);
    }
}