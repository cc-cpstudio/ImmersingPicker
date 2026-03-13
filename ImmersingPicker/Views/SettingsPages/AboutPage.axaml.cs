using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ImmersingPicker.Services.Services;

namespace ImmersingPicker.Views.SettingsPages;

public partial class AboutPage : UserControl
{
    public AboutPage()
    {
        InitializeComponent();
        VersionText.Text = VersionServices.VersionString(VersionServices.CurrentVersion);
    }
}
