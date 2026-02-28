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
    private readonly string _positionFilePath;
    private Timer? _brightnessCheckTimer;

    public SuspendingWindow()
    {
        InitializeComponent();
        
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _positionFilePath = Path.Combine(appDataPath, "ipicker", "Sundry", "SuspendingWindowPos.json");
        
        MovingBtn.SetParentWindow(this);
        MovingBtn.PositionChanged += OnPositionChanged;
        
        OpenMainWindowBtn.Click += OpenMainWindowBtn_Click;
        QuickPickBtn.Click += QuickPickBtn_Click;
        
        LoadPosition();
        
        // 初始化亮度检测定时器
        InitializeBrightnessCheckTimer();
        
        // 初始检测一次
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
        // 位置改变时也更新按钮颜色
        UpdateButtonColors();
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
        _brightnessCheckTimer = new Timer(1000); // 每秒检查一次
        _brightnessCheckTimer.Elapsed += (sender, e) =>
        {
            Dispatcher.UIThread.InvokeAsync(UpdateButtonColors);
        };
        _brightnessCheckTimer.Start();
    }

    private void UpdateButtonColors()
    {
        try
        {
            // 获取浮窗中心位置
            var centerX = Position.X + Width / 2;
            var centerY = Position.Y + Height / 2;
            
            // 检测该位置的亮度
            bool isDarkBackground = IsDarkBackgroundAt(centerX, centerY);
            
            if (isDarkBackground)
            {
                // 暗色背景，使用亮色按钮
                if (MovingBtn.BorderControl != null)
                {
                    MovingBtn.BorderControl.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 255, 255, 255));
                }
                OpenMainWindowBtn.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 255, 255, 255));
                QuickPickBtn.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 255, 255, 255));
                
                // 设置白色图标
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
                // 亮色背景，使用暗色按钮
                if (MovingBtn.BorderControl != null)
                {
                    MovingBtn.BorderControl.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 0, 0, 0));
                }
                OpenMainWindowBtn.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 0, 0, 0));
                QuickPickBtn.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromArgb(51, 0, 0, 0));
                
                // 设置黑色图标
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
            // 出错时使用默认颜色
        }
    }

    private bool IsDarkBackgroundAt(double x, double y)
    {
        try
        {
            // 简化实现：基于窗口位置的亮度检测
            // 实际应用中可以使用屏幕捕获API获取精确颜色
            
            // 计算屏幕亮度值 (0-255)
            int brightness = CalculateScreenBrightness();
            
            // 亮度阈值：低于128认为是暗色背景
            return brightness < 128;
        }
        catch
        {
            return true; // 默认返回暗色背景
        }
    }

    private int CalculateScreenBrightness()
    {
        try
        {
            // 这里使用系统主题来判断
            // 实际应用中可以使用Windows API或其他方法获取屏幕亮度
            
            // 检查系统是否使用暗色主题
            var theme = Avalonia.Application.Current?.RequestedThemeVariant;
            if (theme == Avalonia.Styling.ThemeVariant.Dark)
            {
                return 80; // 暗色主题，返回较低亮度值
            }
            else
            {
                return 200; // 亮色主题，返回较高亮度值
            }
        }
        catch
        {
            return 128; // 默认中等亮度
        }
    }
}

internal class SuspendingWindowPosition
{
    public int PosX { get; set; }
    public int PosY { get; set; }
}
