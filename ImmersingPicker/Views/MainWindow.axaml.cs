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

        Services.NavigationService.Initialize(ContentFrame);
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
            "Home" => Services.NavigationService.ViewType.Home,
            "History" => Services.NavigationService.ViewType.History,
            _ => throw new ArgumentException("Invalid view type")
        };
        Services.NavigationService.NavigateTo(viewType);
    }
}