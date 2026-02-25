using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Interactivity;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;

namespace ImmersingPicker.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();

        TitleBar.Height = 36;
        TitleBar.ExtendsContentIntoTitleBar = false;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;

        Services.MainWindowNavigationService.Initialize(ContentFrame);
    }

    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        MainNavView.SelectedItem = HomePageItem;
    }

    private void MainNavView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs? e)
    {
        if (e.SelectedItem is not NavigationViewItem { Tag: string tag }) return;
        var viewType = tag switch
        {
            "Home" => Services.MainWindowNavigationService.ViewType.Home,
            "History" => Services.MainWindowNavigationService.ViewType.History,
            "Settings" => Services.MainWindowNavigationService.ViewType.Settings,
            _ => throw new ArgumentException("Invalid view type")
        };
        Services.MainWindowNavigationService.NavigateTo(viewType);
        if (viewType == Services.MainWindowNavigationService.ViewType.Settings)
        {
            MainNavView.SelectedItem = HomePageItem;
        }
    }
}