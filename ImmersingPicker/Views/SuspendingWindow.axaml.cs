using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using ImmersingPicker.Views.suspendingWindow;

namespace ImmersingPicker.Views;

public partial class SuspendingWindow : Window
{
    private const int BoundaryMargin = 5;
    private readonly string _positionFilePath;

    public SuspendingWindow()
    {
        InitializeComponent();
        
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _positionFilePath = Path.Combine(appDataPath, "ipicker", "Sundry", "SuspendingWindowPos.json");
        
        MovingBtn.SetParentWindow(this);
        MovingBtn.PositionChanged += OnPositionChanged;
        
        OpenMainWindowBtn.Click += OpenMainWindowBtn_Click;
        
        LoadPosition();
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



    private void OnPositionChanged(object? sender, EventArgs e)
    {
        SavePosition();
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
}

internal class SuspendingWindowPosition
{
    public int PosX { get; set; }
    public int PosY { get; set; }
}
