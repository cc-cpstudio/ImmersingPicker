using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ImmersingPicker.Views.suspendingWindow;

namespace ImmersingPicker.Views;

public partial class EdgeIndicatorWindow : Window
{
    private SuspendingWindow? _parentWindow;
    private bool _isAnimating;
    private bool _isLeftEdge;

    public EdgeIndicatorWindow()
    {
        InitializeComponent();
        Indicator.Clicked += OnIndicatorClicked;
    }

    public void SetParentWindow(SuspendingWindow window)
    {
        _parentWindow = window;
    }

    public void SetPosition(double x, double y, bool isLeftEdge)
    {
        _isLeftEdge = isLeftEdge;
        Position = new PixelPoint((int)x, (int)y);
    }

    public void SetDarkMode(bool isDark)
    {
        Indicator.SetDarkMode(isDark);
    }

    private void OnIndicatorClicked(object? sender, EventArgs e)
    {
        if (_isAnimating || _parentWindow == null) return;

        ShowParentWindow();
    }

    private void ShowParentWindow()
    {
        if (_parentWindow == null) return;
        
        Hide();
        _ = _parentWindow.ShowFromEdgeAsync();
    }

    public void ShowIndicator()
    {
        Show();
    }
}
