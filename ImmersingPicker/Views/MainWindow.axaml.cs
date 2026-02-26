using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Interactivity;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using ImmersingPicker.Services;

namespace ImmersingPicker.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();

        TitleBar.Height = 36;
        TitleBar.ExtendsContentIntoTitleBar = false;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;

        MainWindowNavigationService.Initialize(ContentFrame);
    }

    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        MainNavView.SelectedItem = HomePageItem;
    }

    private void MainNavView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs? e)
    {
        if (e is not { SelectedItem: NavigationViewItem { Tag: string tag } }) return;
        var viewType = tag switch
        {
            "Home" => MainWindowNavigationService.ViewType.Home,
            "History" => MainWindowNavigationService.ViewType.History,
            "Editor" => MainWindowNavigationService.ViewType.Editor,
            "Settings" => MainWindowNavigationService.ViewType.Settings,
            _ => throw new ArgumentException("Invalid view type")
        };
        MainWindowNavigationService.NavigateTo(viewType);
        if (viewType is MainWindowNavigationService.ViewType.Settings or MainWindowNavigationService.ViewType.Editor)
        {
            MainNavView.SelectedItem = HomePageItem;
        }
    }
}