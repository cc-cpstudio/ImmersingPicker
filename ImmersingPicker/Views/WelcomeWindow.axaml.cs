using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Services;

namespace ImmersingPicker.Views;

public partial class WelcomeWindow : AppWindow
{
    public WelcomeWindow()
    {
        InitializeComponent();
        VersionText.Text = VersionServices.VersionString(VersionServices.CurrentVersion);
        StartButton.Background = Brush.Parse(AppSettings.Instance.AppThemeColor);

    }
}