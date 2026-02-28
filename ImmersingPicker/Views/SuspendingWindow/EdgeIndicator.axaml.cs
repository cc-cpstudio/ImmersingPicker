using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace ImmersingPicker.Views.suspendingWindow;

public partial class EdgeIndicator : UserControl
{
    private Border? _indicatorBorder;
    private TextBlock? _textBlock;

    public event EventHandler? Clicked;

    public EdgeIndicator()
    {
        InitializeComponent();
        _indicatorBorder = this.FindControl<Border>("IndicatorBorder");
        _textBlock = this.FindControl<TextBlock>("TextBlock");
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    public void SetDarkMode(bool isDark)
    {
        if (_indicatorBorder == null) return;
        
        if (isDark)
        {
            _indicatorBorder.Background = new SolidColorBrush(Color.FromArgb(204, 255, 255, 255));
            if (_textBlock != null)
            {
                _textBlock.Foreground = Brushes.Black;
            }
        }
        else
        {
            _indicatorBorder.Background = new SolidColorBrush(Color.FromArgb(204, 35, 35, 35));
            if (_textBlock != null)
            {
                _textBlock.Foreground = Brushes.White;
            }
        }
    }
}
