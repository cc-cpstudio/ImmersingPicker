using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace ImmersingPicker;

public partial class MainWindow : Window
{
    private static int _clickAmount;

    public MainWindow()
    {
        InitializeComponent();
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
}