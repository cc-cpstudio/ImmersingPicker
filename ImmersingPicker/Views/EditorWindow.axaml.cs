using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Windowing;
using ImmersingPicker.Views.EditorPages;
using FluentAvalonia.UI.Controls;

namespace ImmersingPicker.Views;

public partial class EditorWindow : AppWindow
{
    private EditPage? _editPage;

    public EditorWindow()
    {
        InitializeComponent();

        TitleBar.Height = 36;
        TitleBar.ExtendsContentIntoTitleBar = false;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        _editPage = new EditorPages.EditPage();
        ContentFrame.Content = _editPage;
    }

    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        if (_editPage != null && _editPage.IsModified)
        {
            e.Cancel = true;
            
            var dialog = new ContentDialog
            {
                Title = "未保存的更改",
                Content = "您有未保存的更改，是否保存？",
                PrimaryButtonText = "保存",
                SecondaryButtonText = "不保存",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary
            };

            var result = await dialog.ShowAsync(this);
            
            if (result == ContentDialogResult.Primary)
            {
                // 保存数据
                try
                {
                    var storageService = new ImmersingPicker.Services.Services.Storage.ClassStorageService();
                    storageService.SaveClasses(ImmersingPicker.Core.Models.Clazz.Classes);
                    _editPage.IsModified = false;
                }
                catch (Exception)
                {
                    // 这里可以添加日志记录
                }
                Close();
            }
            else if (result == ContentDialogResult.Secondary)
            {
                // 不保存，直接关闭
                _editPage.IsModified = false;
                Close();
            }
            // 取消关闭
        }
        
        base.OnClosing(e);
    }
}