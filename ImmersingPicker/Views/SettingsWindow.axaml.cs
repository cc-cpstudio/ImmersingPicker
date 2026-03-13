using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;

namespace ImmersingPicker.Views;

public partial class SettingsWindow : AppWindow
{
    public SettingsWindow()
    {
        InitializeComponent();

        TitleBar.Height = 36;
        TitleBar.ExtendsContentIntoTitleBar = false;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;

        Services.SettingsWindowNavigationService.Initialize(ContentFrame);
    }

    private void MainNavView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is not NavigationViewItem { Tag: string tag }) return;
        var viewType = tag switch
        {
            "BasicSettings" => Services.SettingsWindowNavigationService.ViewType.BasicSettings,
            "PickerSettings" => Services.SettingsWindowNavigationService.ViewType.PickerSettings,
            "HomePageSettings" => Services.SettingsWindowNavigationService.ViewType.HomePageSettings,
            "About" => Services.SettingsWindowNavigationService.ViewType.About,
            _ => throw new ArgumentException("Invalid view type")
        };
        Services.SettingsWindowNavigationService.NavigateTo(viewType);
    }
}