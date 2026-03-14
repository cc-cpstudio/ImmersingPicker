using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Views.WelcomePages;

public partial class ClazzInitializationPage : UserControl
{
    public ClazzInitializationPage()
    {
        InitializeComponent();
        NextButton.Background = Brush.Parse(AppSettings.Instance.AppThemeColor);
    }

    private void NextButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (FirstClazzNameInput.Text == "")
        {
            var dialog = new ContentDialog
            {
                Title = "请输入班级名称",
                Content = "班级名称不能为空",
                CloseButtonText = "确定"
            };
        }
        else
        {

        }
    }
}