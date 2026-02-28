using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace ImmersingPicker.Views.suspendingWindow;

public partial class MovingControlButton : UserControl
{
    private bool _isDragging;
    private Point _startPoint;
    private Window? _parentWindow;
    private Border? _borderControl;

    public event EventHandler? PositionChanged;

    public MovingControlButton()
    {
        InitializeComponent();
        _borderControl = this.FindControl<Border>("Border");
    }

    public Border? BorderControl => _borderControl;

    public void SetParentWindow(Window window)
    {
        _parentWindow = window;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_parentWindow == null) return;
        
        _isDragging = true;
        _startPoint = e.GetPosition(_parentWindow);
        e.Pointer.Capture((IInputElement)sender!);
        e.Handled = true;
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isDragging || _parentWindow == null) return;

        var currentPoint = e.GetPosition(_parentWindow);
        var delta = currentPoint - _startPoint;

        var proposedX = _parentWindow.Position.X + delta.X;
        var proposedY = _parentWindow.Position.Y + delta.Y;

        if (_parentWindow is SuspendingWindow suspendingWindow)
        {
            suspendingWindow.ConstrainToScreen(proposedX, proposedY, out var constrainedX, out var constrainedY);
            _parentWindow.Position = new Avalonia.PixelPoint((int)constrainedX, (int)constrainedY);
        }
        else
        {
            _parentWindow.Position = new Avalonia.PixelPoint((int)proposedX, (int)proposedY);
        }
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!_isDragging) return;

        _isDragging = false;
        e.Pointer.Capture(null);
        PositionChanged?.Invoke(this, EventArgs.Empty);
    }
}
