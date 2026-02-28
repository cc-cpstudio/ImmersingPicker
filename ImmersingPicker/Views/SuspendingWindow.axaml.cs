using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using Avalonia.Threading;
using Avalonia.VisualTree;
using ImmersingPicker.Views.suspendingWindow;

namespace ImmersingPicker.Views;

public partial class SuspendingWindow : Window
{
    private const int BoundaryMargin = 5;
    private const int EdgeThreshold = 10;
    private const int AutoHideDelayMs = 2000;
    
    private readonly string _positionFilePath;
    private Timer? _brightnessCheckTimer;
    private Timer? _edgeDetectionTimer;
    private EdgeIndicatorWindow? _edgeIndicator;
    private bool _isHidden;
    private bool _isAnimating;
    private bool _isAtLeftEdge;
    private DateTime _edgeEnterTime;
    private bool _isWaitingForAutoHide;
    private double _hiddenX;
    private double _visibleX;
    private double _storedY;

    public event EventHandler? DragEnded;

    public SuspendingWindow()
    {
        InitializeComponent();
        
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _positionFilePath = Path.Combine(appDataPath, "ipicker", "Sundry", "SuspendingWindowPos.json");
        
        MovingBtn.SetParentWindow(this);
        MovingBtn.PositionChanged += OnPositionChanged;
        MovingBtn.DragStarted += OnDragStarted;
        MovingBtn.DragEnded += OnDragEnded;
        
        OpenMainWindowBtn.Click += OpenMainWindowBtn_Click;
        QuickPickBtn.Click += QuickPickBtn_Click;
        
        LoadPosition();
        
        InitializeBrightnessCheckTimer();
        InitializeEdgeDetectionTimer();
        
        UpdateButtonColors();
    }

    private void OpenMainWindowBtn_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        OpenMainWindow();
    }

    private void OpenMainWindowBtn_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        OpenMainWindow();
    }

    private void OpenMainWindow()
    {
        if (App.Current is App app)
        {
            app.ShowMainWindow(null, EventArgs.Empty);
        }
    }

    private void QuickPickBtn_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        QuickPick();
    }

    private void QuickPickBtn_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        QuickPick();
    }

    private void QuickPick()
    {
        var quickPickWindow = new QuickPickWindow();
        quickPickWindow.Show();
    }

    private void OnPositionChanged(object? sender, EventArgs e)
    {
        SavePosition();
        UpdateButtonColors();
    }

    private void OnDragStarted(object? sender, EventArgs e)
    {
        _isWaitingForAutoHide = false;
    }

    private void OnDragEnded(object? sender, EventArgs e)
    {
        DragEnded?.Invoke(this, EventArgs.Empty);
    }

    public void ConstrainToScreen(double proposedX, double proposedY, out double constrainedX, out double constrainedY)
    {
        var screen = Screens.ScreenFromWindow(this);
        if (screen == null)
        {
            constrainedX = proposedX;
            constrainedY = proposedY;
            return;
        }

        var workingArea = screen.WorkingArea;
        const double windowWidth = 60;
        const double windowHeight = 120;

        constrainedX = Math.Max(workingArea.X + BoundaryMargin, Math.Min(proposedX, workingArea.X + workingArea.Width - windowWidth - BoundaryMargin));
        constrainedY = Math.Max(workingArea.Y + BoundaryMargin, Math.Min(proposedY, workingArea.Y + workingArea.Height - windowHeight - BoundaryMargin));
    }

    private void InitializeEdgeDetectionTimer()
    {
        _edgeDetectionTimer = new Timer(100);
        _edgeDetectionTimer.Elapsed += OnEdgeDetectionTimerElapsed;
        _edgeDetectionTimer.Start();
    }

    private void OnEdgeDetectionTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Dispatcher.UIThread.Post(CheckEdgeAndAutoHide);
    }

    private void CheckEdgeAndAutoHide()
    {
        if (_isAnimating || _isHidden) return;

        var screen = Screens.ScreenFromWindow(this);
        if (screen == null) return;

        var workingArea = screen.WorkingArea;
        var currentX = Position.X;
        var currentY = Position.Y;

        bool atLeftEdge = currentX <= workingArea.X + EdgeThreshold;
        bool atRightEdge = currentX + Width >= workingArea.X + workingArea.Width - EdgeThreshold;
        bool atEdge = atLeftEdge || atRightEdge;

        if (atEdge)
        {
            if (!_isWaitingForAutoHide)
            {
                _isAtLeftEdge = atLeftEdge;
                _storedY = currentY;
                _isWaitingForAutoHide = true;
                _edgeEnterTime = DateTime.Now;
            }
            else
            {
                var elapsed = (DateTime.Now - _edgeEnterTime).TotalMilliseconds;
                if (elapsed >= AutoHideDelayMs)
                {
                    _isWaitingForAutoHide = false;
                    _ = HideToEdgeAsync();
                }
            }
        }
        else
        {
            _isWaitingForAutoHide = false;
        }
    }

    private async System.Threading.Tasks.Task HideToEdgeAsync()
    {
        if (_isAnimating) return;

        _isAnimating = true;

        var screen = Screens.ScreenFromWindow(this);
        if (screen == null)
        {
            _isAnimating = false;
            return;
        }

        var workingArea = screen.WorkingArea;
        _visibleX = Position.X;
        _storedY = Position.Y;

        CreateEdgeIndicator();

        _hiddenX = _isAtLeftEdge 
            ? workingArea.X - Width + 5
            : workingArea.X + workingArea.Width - 5;

        await AnimateWindowPositionAsync(_hiddenX, _storedY, 300);

        Hide();
        _isHidden = true;
        _isAnimating = false;

        if (_edgeIndicator != null)
        {
            var indicatorX = _isAtLeftEdge 
                ? workingArea.X + BoundaryMargin
                : workingArea.X + workingArea.Width - 40 - BoundaryMargin;
            var indicatorY = _storedY + (Height - 40) / 2;

            _edgeIndicator.SetPosition(indicatorX, indicatorY, _isAtLeftEdge);
            _edgeIndicator.SetDarkMode(IsDarkBackgroundAt(indicatorX + 20, indicatorY + 20));
            _edgeIndicator.ShowIndicator();
        }
    }

    private void CreateEdgeIndicator()
    {
        if (_edgeIndicator != null) return;

        _edgeIndicator = new EdgeIndicatorWindow();
        _edgeIndicator.SetParentWindow(this);
        _edgeIndicator.Show();
        _edgeIndicator.Hide();
    }

    public async System.Threading.Tasks.Task ShowFromEdgeAsync()
    {
        if (_isAnimating || !_isHidden) return;

        _isAnimating = true;

        if (_edgeIndicator != null)
        {
            _edgeIndicator.Hide();
        }

        Show();
        
        Position = new PixelPoint((int)_hiddenX, (int)_storedY);
        
        await AnimateWindowPositionAsync(_visibleX, _storedY, 300);

        _isHidden = false;
        _isAnimating = false;
        _isWaitingForAutoHide = false;
    }

    private async System.Threading.Tasks.Task AnimateWindowPositionAsync(double targetX, double targetY, int durationMs)
    {
        var startX = Position.X;
        var startY = Position.Y;
        var startTime = DateTime.Now;
        var endTime = startTime.AddMilliseconds(durationMs);

        while (DateTime.Now < endTime)
        {
            var progress = (DateTime.Now - startTime).TotalMilliseconds / durationMs;
            progress = EaseOutCubic(Math.Min(progress, 1.0));

            var currentX = startX + (targetX - startX) * progress;
            var currentY = startY + (targetY - startY) * progress;

            Position = new PixelPoint((int)currentX, (int)currentY);

            await System.Threading.Tasks.Task.Delay(16);
        }

        Position = new PixelPoint((int)targetX, (int)targetY);
    }

    private double EaseOutCubic(double t)
    {
        return 1 - Math.Pow(1 - t, 3);
    }

    private void LoadPosition()
    {
        try
        {
            if (File.Exists(_positionFilePath))
            {
                var json = File.ReadAllText(_positionFilePath);
                var position = JsonSerializer.Deserialize<SuspendingWindowPosition>(json);
                if (position != null)
                {
                    Position = new PixelPoint(position.PosX, position.PosY);
                }
            }
        }
        catch
        {
        }
    }

    private void SavePosition()
    {
        try
        {
            var directory = Path.GetDirectoryName(_positionFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var position = new SuspendingWindowPosition
            {
                PosX = Position.X,
                PosY = Position.Y
            };

            var json = JsonSerializer.Serialize(position, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_positionFilePath, json);
        }
        catch
        {
        }
    }

    private void InitializeBrightnessCheckTimer()
    {
        _brightnessCheckTimer = new Timer(1000);
        _brightnessCheckTimer.Elapsed += OnBrightnessCheckTimerElapsed;
        _brightnessCheckTimer.Start();
    }

    private void OnBrightnessCheckTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Dispatcher.UIThread.Post(UpdateButtonColors);
    }

    private void UpdateButtonColors()
    {
        try
        {
            var centerX = Position.X + Width / 2;
            var centerY = Position.Y + Height / 2;
            
            bool isDarkBackground = IsDarkBackgroundAt(centerX, centerY);
            
            if (isDarkBackground)
            {
                if (MovingBtn.BorderControl != null)
                {
                    MovingBtn.BorderControl.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 255, 255, 255));
                }
                OpenMainWindowBtn.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 255, 255, 255));
                QuickPickBtn.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 255, 255, 255));
                
                if (MovingBtn.BorderControl != null)
                {
                    foreach (var child in MovingBtn.BorderControl.GetVisualChildren())
                    {
                        if (child is TextBlock textBlock)
                        {
                            textBlock.Foreground = Avalonia.Media.Brushes.White;
                        }
                    }
                }
            }
            else
            {
                if (MovingBtn.BorderControl != null)
                {
                    MovingBtn.BorderControl.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 0, 0, 0));
                }
                OpenMainWindowBtn.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 0, 0, 0));
                QuickPickBtn.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 0, 0, 0));
                
                if (MovingBtn.BorderControl != null)
                {
                    foreach (var child in MovingBtn.BorderControl.GetVisualChildren())
                    {
                        if (child is TextBlock textBlock)
                        {
                            textBlock.Foreground = Avalonia.Media.Brushes.Black;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    private bool IsDarkBackgroundAt(double x, double y)
    {
        try
        {
            int brightness = CalculateScreenBrightness();
            return brightness < 128;
        }
        catch
        {
            return true;
        }
    }

    private int CalculateScreenBrightness()
    {
        try
        {
            var theme = Avalonia.Application.Current?.RequestedThemeVariant;
            if (theme == Avalonia.Styling.ThemeVariant.Dark)
            {
                return 80;
            }
            else
            {
                return 200;
            }
        }
        catch
        {
            return 128;
        }
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (_edgeIndicator != null)
        {
            _edgeIndicator.Close();
        }
        
        if (_brightnessCheckTimer != null)
        {
            _brightnessCheckTimer.Stop();
            _brightnessCheckTimer.Dispose();
        }
        
        if (_edgeDetectionTimer != null)
        {
            _edgeDetectionTimer.Stop();
            _edgeDetectionTimer.Dispose();
        }
        
        base.OnClosing(e);
    }
}

internal class SuspendingWindowPosition
{
    public int PosX { get; set; }
    public int PosY { get; set; }
}
