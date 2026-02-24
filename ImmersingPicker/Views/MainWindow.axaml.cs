using System;
using System.Diagnostics;
using Avalonia.Interactivity;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;

namespace ImmersingPicker.Views;

public partial class MainWindow : AppWindow
{
    private static int _clickAmount;

    public MainWindow()
    {
        InitializeComponent();

        TitleBar.Height = 36;
        TitleBar.ExtendsContentIntoTitleBar = false;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }

    static MainWindow()
    {
        _clickAmount = 0;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Click!" + _clickAmount);
        _clickAmount ++;
    }

    private void MainNavView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is NavigationViewItem { Tag: string tag })
        {
            var viewType = tag switch
            {
                "Home" => Services.NavigationService.ViewType.Home,
                "History" => Services.NavigationService.ViewType.History,
                _ => throw new ArgumentException("Invalid view type")
            };
            Services.NavigationService.NavigateTo(viewType);
        }
    }
}