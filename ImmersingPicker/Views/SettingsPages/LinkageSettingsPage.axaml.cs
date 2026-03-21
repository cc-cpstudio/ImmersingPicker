using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ImmersingPicker.Controls;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Views.SettingsPages;

public partial class LinkageSettingsPage : SettingsPageBase
{
    public LinkageSettingsPage()
    {
        PageTitle.Text = "联动设置";
        InitializeComponent();
        LoadSettings();
        UpdateControlsState();
    }

    protected override void LoadSettings()
    {
        EnableClassIslandLinkage.IsChecked = AppSettings.EnableClassIslandLinkage;
        EnableDisablingAfterClasses.IsChecked = AppSettings.EnableDisablingAfterClasses;
    }

    protected override void UpdateControlsState()
    {
        bool classIslandLinkageEnabled = EnableClassIslandLinkage.IsChecked ?? false;
        EnableDisablingAfterClasses.IsEnabled = classIslandLinkageEnabled;
    }

    private void EnableClassIslandLinkage_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        AppSettings.EnableClassIslandLinkage = EnableClassIslandLinkage.IsChecked ?? false;
        UpdateControlsState();
    }

    private void EnableDisablingAfterClasses_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        AppSettings.EnableDisablingAfterClasses = EnableDisablingAfterClasses.IsChecked ?? false;
    }
}