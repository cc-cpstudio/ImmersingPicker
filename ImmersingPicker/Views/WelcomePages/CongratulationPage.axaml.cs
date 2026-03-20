using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Views.WelcomePages;

public partial class CongratulationPage : UserControl
{
    public CongratulationPage()
    {
        InitializeComponent();
        NextButton.Background = Brush.Parse(AppSettings.Instance.AppThemeColor);
    }

    private void NextButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current is App app)
        {
            app.CompleteWelcomeSetup();
        }
    }
}