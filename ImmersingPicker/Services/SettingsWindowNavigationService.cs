using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace ImmersingPicker.Services;

public class SettingsWindowNavigationService
{
    private static Frame? _mainContentFrame;

    public static void Initialize(Frame mainFrame)
    {
        _mainContentFrame = mainFrame;
    }

    public static void NavigateTo(Type viewType)
    {
        if (_mainContentFrame != null)
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
            ViewType.BasicSettings => typeof(Views.SettingsPages.BasicSettingsPage),
            ViewType.PickerSettings => typeof(Views.SettingsPages.PickerSettingsPage),
            ViewType.About => typeof(Views.SettingsPages.AboutPage),
            _ => throw new ArgumentException("Invalid view type")
        };
        NavigateTo(targetType);
    }

    public enum ViewType
    {
        BasicSettings,
        PickerSettings,
        About
    }
}