using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Controls;
using FluentAvalonia.UI.Controls;
using ImmersingPicker.Core;

namespace ImmersingPicker.Views.MainPages;

public partial class HistoryPage : UserControl
{
    private Clazz? _clazz;
    private FilterCriteria? _currentFilter;
    private FilterDialog? _filterDialog;

    public HistoryPage()
    {
        InitializeComponent();
        Clazz.CurrentClassChanged += OnCurrentClassChanged;
        ClearHistoryButton.Click += OnClearHistoryButtonClick;
        FilterButton.Click += OnFilterButtonClick;
        ResetFilterButton.Click += OnResetFilterButtonClick;
        ClearFilterButton.Click += OnResetFilterButtonClick;
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

        var histories = _clazz.Histories.AsEnumerable();

        if (_currentFilter != null && _currentFilter.HasAnyFilter())
        {
            histories = ApplyFilter(histories);
        }

        var sortedHistories = histories.OrderByDescending(h => h.CreateTime);

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
            var noHistoryText = new TextBlock
            {
                Text = _currentFilter != null && _currentFilter.HasAnyFilter() 
                    ? "没有符合筛选条件的历史记录" 
                    : "暂无历史记录",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                FontSize = 32,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 100, 0, 0)
            };
            HistoryItemsContainer.Children.Add(noHistoryText);
        }
    }

    private IEnumerable<History> ApplyFilter(IEnumerable<History> histories)
    {
        if (_currentFilter == null) return histories;

        var filtered = histories;

        if (_currentFilter.StartDate.HasValue)
        {
            var startDateTime = _currentFilter.StartDate.Value;
            filtered = filtered.Where(h => h.CreateTime >= startDateTime);
        }

        if (_currentFilter.EndDate.HasValue)
        {
            var endDateTime = _currentFilter.EndDate.Value;
            filtered = filtered.Where(h => h.CreateTime <= endDateTime);
        }

        if (_currentFilter.MinCount > 0)
        {
            filtered = filtered.Where(h => h.Students.Count >= _currentFilter.MinCount);
        }

        if (_currentFilter.MaxCount > 0)
        {
            filtered = filtered.Where(h => h.Students.Count <= _currentFilter.MaxCount);
        }

        if (_currentFilter.SelectedStudents.Count > 0)
        {
            filtered = filtered.Where(h => 
                h.Students.Any(s => _currentFilter.SelectedStudents.Any(ss => ss.Id == s.Id)));
        }

        return filtered;
    }

    private async void OnFilterButtonClick(object? sender, EventArgs e)
    {
        if (_clazz == null) return;

        _filterDialog = new FilterDialog(_clazz.Students);

        if (_currentFilter != null && _currentFilter.HasAnyFilter())
        {
            _filterDialog.SetFilterCriteria(_currentFilter);
        }

        var dialog = new ContentDialog
        {
            Title = "筛选历史记录",
            Content = _filterDialog,
            PrimaryButtonText = "确定",
            CloseButtonText = "取消"
        };

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            _currentFilter = _filterDialog.GetFilterCriteria();
            UpdateFilterSummary();
            LoadHistories();
        }
    }

    private void OnResetFilterButtonClick(object? sender, EventArgs e)
    {
        _currentFilter = null;
        _filterDialog?.ResetFilter();
        UpdateFilterSummary();
        LoadHistories();
    }

    private void UpdateFilterSummary()
    {
        var hasFilter = _currentFilter != null && _currentFilter.HasAnyFilter();
        
        ResetFilterButton.IsEnabled = hasFilter;
        FilterSummaryBorder.IsVisible = hasFilter;

        if (hasFilter && _currentFilter != null)
        {
            FilterSummaryText.Text = _currentFilter.GetSummary();
        }
        else
        {
            FilterSummaryText.Text = string.Empty;
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
        
        var dialog = new ContentDialog
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
        
        var dialog = new ContentDialog
        {
            Title = "清空历史记录",
            Content = "请选择要执行的操作：",
            PrimaryButtonText = "清空历史记录",
            SecondaryButtonText = "清空历史记录并重置权重",
            CloseButtonText = "取消"
        };
        
        var result = await dialog.ShowAsync();
        
        if (result == ContentDialogResult.Primary || 
            result == ContentDialogResult.Secondary)
        {
            var confirmDialog = new ContentDialog
            {
                Title = "确认操作",
                Content = "确定要执行此操作吗？此操作不可恢复。",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消"
            };
            
            var confirmResult = await confirmDialog.ShowAsync();
            
            if (confirmResult == ContentDialogResult.Primary)
            {
                if (result == ContentDialogResult.Primary)
                {
                    _clazz.Histories.Clear();
                }
                else
                {
                    _clazz.Histories.Clear();
                    foreach (var student in _clazz.Students)
                    {
                        student.resetHistories();
                    }
                }
                
                bool success = false;
                try
                {
                    var storageService = new ImmersingPicker.Services.Services.Storage.ClassStorageService();
                    storageService.SaveClasses(Clazz.Classes);
                    success = true;
                }
                catch (Exception ex)
                {
                }
                
                LoadHistories();
                
                var successDialog = new ContentDialog
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
