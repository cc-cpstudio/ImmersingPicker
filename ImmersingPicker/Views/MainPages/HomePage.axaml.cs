using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ImmersingPicker.Core;

namespace ImmersingPicker.Views.MainPages;

public partial class HomePage : UserControl
{
    private static int _amountForPicking;

    private int AmountForPicking
    {
        get => _amountForPicking;
        set
        {
            if (value < 1 || value > (Clazz.GetCurrentClazz() ?? new Clazz()).Students.Count)
            {
                throw new ArgumentException("Invalid amount");
            }
            else
            {
                _amountForPicking = value;
                PickButton.Content = $"共{_amountForPicking}人  开始抽选！";
            }
        }
    }

    public HomePage()
    {
        _amountForPicking = 1;
        InitializeComponent();
    }

    private void MinusButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AmountForPicking = AmountForPicking - 1;
    }

    private void PickButton_OnClick(object? sender, RoutedEventArgs e)
    {

    }
}