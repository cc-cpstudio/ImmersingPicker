using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Windowing;

namespace ImmersingPicker.Views;

public partial class EditorWindow : AppWindow
{
    public EditorWindow()
    {
        InitializeComponent();

        TitleBar.Height = 36;
        TitleBar.ExtendsContentIntoTitleBar = false;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        ContentFrame.Content = new EditorPages.EditPage();
    }
}