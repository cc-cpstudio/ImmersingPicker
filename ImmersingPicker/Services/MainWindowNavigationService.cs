using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using ImmersingPicker.Abstractions;
using ImmersingPicker.Views;

namespace ImmersingPicker.Services;

public class MainWindowNavigationService
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
        if (viewType == ViewType.Settings)
        {
            OpenSettingsWindow();
            return;
        }
        if (viewType == ViewType.Editor)
        {
            OpenEditorWindow();
            return;
        }

        Type targetType = viewType switch
        {
            ViewType.Home => typeof(Views.MainPages.HomePage),
            ViewType.History => typeof(Views.MainPages.HistoryPage),
            _ => throw new ArgumentException("Invalid view type")
        };
        NavigateTo(targetType);
    }

    public static void OpenSettingsWindow()
    {
        var settingsWindow = new SettingsWindow();
        settingsWindow.Show();
    }

    public static void OpenEditorWindow()
    {
        var editorWindow = new EditorWindow();
        editorWindow.Show();
    }

    public enum ViewType
    {
        Home, History, Settings,
        Editor
    }
}