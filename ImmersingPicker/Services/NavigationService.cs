using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace ImmersingPicker.Services;

public class NavigationService
{
    private static Frame? _mainContentFrame;

    public static void Initialize(Frame mainFrame)
    {
        _mainContentFrame = mainFrame;
    }

    public static void NavigateTo(Type viewType)
    {
        if (_mainContentFrame == null)
        {

        }
        else
        {
            if (Activator.CreateInstance(viewType) is UserControl view)
            {
                _mainContentFrame.Content = view;
            }
        }
    }

    public static void NavigateTo(ViewType viewType)
    {
        Type targetType = viewType switch
        {
            ViewType.Home => typeof(Views.MainPages.HomePage),
            ViewType.History => typeof(Views.MainPages.HistoryPage),
            _ => throw new ArgumentException("Invalid view type")
        };
        NavigateTo(targetType);
    }

    public enum ViewType
    {
        Home, History
    }
}