using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace ImmersingPicker.Abstractions;

public interface INavigationService
{
    protected static Frame? _mainContentFrame;

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

    public static abstract void NavigateTo(ViewType viewType);

    public enum ViewType;
}