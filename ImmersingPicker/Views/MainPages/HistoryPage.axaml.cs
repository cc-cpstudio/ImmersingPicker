using System;
using System.Linq;
using Avalonia.Controls;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Controls;
using FluentAvalonia.UI.Controls;

namespace ImmersingPicker.Views.MainPages;

public partial class HistoryPage : UserControl
{
    private Clazz? _clazz;

    public HistoryPage()
    {
        InitializeComponent();
        Clazz.CurrentClassChanged += OnCurrentClassChanged;
        ClearHistoryButton.Click += OnClearHistoryButtonClick;
        _clazz = Clazz.GetCurrentClazz();
        if (_clazz != null)
        {
            _clazz.HistoriesChanged += OnHistoriesChanged;
            TitleText.Text = $"班级 {_clazz.Name} 的历史记录";
            LoadHistories();
        }
    }

    public void RefreshClazz()
    {
        if (_clazz != null)
        {
            _clazz.HistoriesChanged -= OnHistoriesChanged;
        }
        _clazz = Clazz.GetCurrentClazz();
        if (_clazz == null) return;
        _clazz.HistoriesChanged += OnHistoriesChanged;
        TitleText.Text = $"班级 {_clazz.Name} 的历史记录";
        LoadHistories();
    }

    private void OnHistoriesChanged()
    {
        LoadHistories();
    }

    private void OnCurrentClassChanged()
    {
        RefreshClazz();
    }

    private void LoadHistories()
    {
        if (_clazz == null) return;

        HistoryItemsContainer.Children.Clear();

        // 按时间倒序排序历史记录
        var sortedHistories = _clazz.Histories.OrderByDescending(h => h.CreateTime);

        if (sortedHistories.Any())
        {
            foreach (var history in sortedHistories)
            {
                var historyItem = new HistoryItem(history);
                historyItem.Clicked += OnHistoryItemClicked;
                HistoryItemsContainer.Children.Add(historyItem);
            }
        }
        else
        {
            // 显示暂无历史记录的提示
            var noHistoryText = new TextBlock
            {
                Text = "暂无历史记录",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                FontSize = 32,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 100, 0, 0)
            };
            HistoryItemsContainer.Children.Add(noHistoryText);
        }
    }
    
    private async void OnHistoryItemClicked(object? sender, EventArgs e)
    {
        if (sender is not HistoryItem historyItem) return;
        var history = historyItem.History;
        if (history == null) return;
        
        string studentsList = string.Join("\n", history.Students.Select(s => $"{s.Id} {s.Name}"));
        string content = $"时间：{history.CreateTime.ToString()}\n\n" +
                        $"抽选器：{history.Selector}\n\n" +
                        $"抽选到的学生：\n{studentsList}";
        
        var dialog = new FluentAvalonia.UI.Controls.ContentDialog
        {
            Title = "抽选历史详情",
            Content = content,
            CloseButtonText = "确定"
        };
        
        await dialog.ShowAsync();
    }
    
    private async void OnClearHistoryButtonClick(object? sender, EventArgs e)
    {
        if (_clazz == null) return;
        
        var dialog = new FluentAvalonia.UI.Controls.ContentDialog
        {
            Title = "清空历史记录",
            Content = "请选择要执行的操作：",
            PrimaryButtonText = "清空历史记录",
            SecondaryButtonText = "清空历史记录并重置权重",
            CloseButtonText = "取消"
        };
        
        var result = await dialog.ShowAsync();
        
        if (result == FluentAvalonia.UI.Controls.ContentDialogResult.Primary || 
            result == FluentAvalonia.UI.Controls.ContentDialogResult.Secondary)
        {
            // 弹出挽留提示
            var confirmDialog = new FluentAvalonia.UI.Controls.ContentDialog
            {
                Title = "确认操作",
                Content = "确定要执行此操作吗？此操作不可恢复。",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消"
            };
            
            var confirmResult = await confirmDialog.ShowAsync();
            
            if (confirmResult == FluentAvalonia.UI.Controls.ContentDialogResult.Primary)
            {
                // 执行操作
                if (result == FluentAvalonia.UI.Controls.ContentDialogResult.Primary)
                {
                    // 清空历史记录
                    _clazz.Histories.Clear();
                }
                else
                {
                    // 清空历史记录并重置权重
                    _clazz.Histories.Clear();
                    foreach (var student in _clazz.Students)
                    {
                        student.resetHistories();
                    }
                }
                
                // 保存数据
                bool success = false;
                try
                {
                    var storageService = new ImmersingPicker.Services.Services.Storage.ClassStorageService();
                    storageService.SaveClasses(ImmersingPicker.Core.Models.Clazz.Classes);
                    success = true;
                }
                catch (Exception ex)
                {
                    // 这里可以添加日志记录
                }
                
                // 刷新历史记录页面
                LoadHistories();
                
                // 弹出提示
                var successDialog = new FluentAvalonia.UI.Controls.ContentDialog
                {
                    Title = success ? "操作成功" : "操作失败",
                    Content = success ? "历史记录已成功清空" : "清空历史记录时发生错误",
                    CloseButtonText = "确定"
                };
                
                await successDialog.ShowAsync();
            }
        }
    }
}