using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using ImmersingPicker.Core;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Controls;

public partial class SeatGrid : UserControl
{
    private Dictionary<int, int> _rowIndexMap = new();
    private Dictionary<int, int> _columnIndexMap = new();

    private Clazz? _clazz;

    private Dictionary<int, Button> _seats = new();

    public SeatGrid()
    {
        InitializeComponent();
        RefreshClazz();
        Clazz.CurrentClassChanged += RefreshClazz;
    }

    public void RefreshClazz()
    {
        if (_clazz != null) {
            _clazz.StudentListChanged -= RefreshStudents;
        }
        _clazz = Clazz.GetCurrentClazz();
        if (_clazz != null)
        {
            _clazz.StudentListChanged += RefreshStudents;
        }
        RefreshStudents();
    }

    public void RefreshStudents()
    {
        _seats.Clear();
        _rowIndexMap.Clear();
        _columnIndexMap.Clear();

        grid.Children.Clear();
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();

        if (_clazz == null)
            SetTip("当前暂无班级");
        else if (_clazz.Students.Count <= 0)
            SetTip($"班级 {_clazz.Name} 中暂无学生");
        else
        {
            // 首先收集所有唯一的 SeatRow 和 SeatColumn 值
            var uniqueRows = _clazz.Students
                .Where(s => s.SeatRow > 0)
                .Select(s => s.SeatRow)
                .Distinct()
                .OrderBy(row => row)
                .ToList();

            var uniqueColumns = _clazz.Students
                .Where(s => s.SeatColumn > 0)
                .Select(s => s.SeatColumn)
                .Distinct()
                .OrderBy(col => col)
                .ToList();

            // 为每个唯一的 SeatRow 和 SeatColumn 值分配一个连续的索引
            for (int i = 0; i < uniqueRows.Count; i++)
            {
                _rowIndexMap[uniqueRows[i]] = i;
                grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            }

            for (int i = 0; i < uniqueColumns.Count; i++)
            {
                _columnIndexMap[uniqueColumns[i]] = i;
                grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            }

            // 然后创建并添加所有座位按钮
            foreach (var student in _clazz.Students)
            {
                if (student.SeatRow < 0 || student.SeatColumn < 1) continue;

                var button = new Button
                {
                    Content = $"{student.Id} {student.Name}",
                    Width = 80,
                    Height = 48,
                    Margin = new Thickness(5),
                    BorderThickness = new Thickness(2),
                    BorderBrush = Avalonia.Media.Brushes.Gray,
                };
                button.Click += async (sender, e) =>
                {
                    var dialog = new ContentDialog
                    {
                        Title = $"当前学生：{student.Name}",
                        Content = $"学号：{student.Id}",
                        CloseButtonText = "确定"
                    };
                    await dialog.ShowAsync();
                };

                _seats[student.Id] = button;
                grid.Children.Add(button);
                
                // 使用映射后的索引来设置按钮的位置
                Grid.SetRow(button, _rowIndexMap[student.SeatRow]);
                Grid.SetColumn(button, _columnIndexMap[student.SeatColumn]);
            }
        }

        void SetTip(string tip)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

            TextBlock tipTextBlock = new TextBlock
            {
                Text = tip,
                FontSize = 48,
                FontWeight = Avalonia.Media.FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            grid.Children.Add(tipTextBlock);
            Grid.SetRow(tipTextBlock, 0);
            Grid.SetColumn(tipTextBlock, 0);
        }
    }

    public void Select(Student student)
    {
        if (_seats.TryGetValue(student.Id, out var seat))
        {
            seat.BorderBrush = Avalonia.Media.Brushes.LimeGreen;
        }
    }

    public void Deselect(Student student)
    {
        if (_seats.TryGetValue(student.Id, out var seat))
        {
            seat.BorderBrush = Avalonia.Media.Brushes.Gray;
        }
    }

    public void DeselectAll()
    {
        if (_clazz == null) return;
        foreach (Student student in _clazz.Students)
        {
            Deselect(student);
        }
    }
}