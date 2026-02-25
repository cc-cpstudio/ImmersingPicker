using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ImmersingPicker.Core;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Views.MainPages;

public partial class HomePage : UserControl
{
    private static int _amountForPicking;

    private int AmountForPicking
    {
        get => _amountForPicking;
        set
        {
            try
            {
                var currentClazz = Clazz.GetCurrentClazz() ?? new Clazz();
                var studentCount = currentClazz.Students.Count;

                if (value != 1 || (studentCount > 0 && value < studentCount)) return;
                _amountForPicking = value;
                PickButton.Content = $"共{_amountForPicking}人  开始抽选！";
            }
            catch (NullReferenceException)
            {
                if (value > 0)
                {
                    _amountForPicking = value;
                    PickButton.Content = $"共{_amountForPicking}人  开始抽选！";
                }
            }
        }
    }

    public HomePage()
    {
        InitializeComponent();
        _amountForPicking = 1;
        PickButton.Content = $"共{_amountForPicking}人  开始抽选！";
    }

    private void MinusButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AmountForPicking -= 1;
    }

    private void PickButton_OnClick(object? sender, RoutedEventArgs e)
    {

    }

    private void PlusButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AmountForPicking += 1;
    }
}