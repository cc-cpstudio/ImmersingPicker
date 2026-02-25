using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using ImmersingPicker.Core;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Controls;

public partial class SeatGrid : UserControl
{
    private Dictionary<int, RowDefinition> _rows = new();
    private Dictionary<int, ColumnDefinition> _columns = new();

    private Clazz? _clazz;

    private Dictionary<int, Seat> _seats = new();

    public SeatGrid()
    {
        InitializeComponent();
        RefreshClazz();
    }

    public void RefreshClazz()
    {
        _clazz = Clazz.GetCurrentClazz();
        RefreshStudents();
    }

    public void RefreshStudents()
    {
        _seats.Clear();
        _rows.Clear();
        _columns.Clear();

        grid.Children.Clear();
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();

        if (_clazz == null)
            SetTip("当前暂无班级");
        else if (_clazz.Students.Count <= 0)
            SetTip($"班级 {_clazz.Name} 中暂无学生");
        else
        {
            foreach (Student student in _clazz!.Students)
            {
                if (student.SeatRow < 0 || student.SeatColumn < 1) continue;
                if (!_rows.ContainsKey(student.SeatRow))
                {
                    _rows.Add(student.SeatRow, new RowDefinition(GridLength.Auto));
                    grid.RowDefinitions.Add(_rows[student.SeatRow]);
                }
                if (!_columns.ContainsKey(student.SeatColumn))
                {
                    _columns.Add(student.SeatColumn, new ColumnDefinition(GridLength.Auto));
                    grid.ColumnDefinitions.Add(_columns[student.SeatColumn]);
                }

                if (!_seats.ContainsKey(student.Id))
                {
                    _seats.Add(student.Id, new Seat(student));
                    grid.Children.Add(_seats[student.Id]);
                    Grid.SetRow(_seats[student.Id], student.SeatRow);
                    Grid.SetColumn(_seats[student.Id], student.SeatColumn);
                }
            }
        }

        void SetTip(string tip)
        {
            _rows.Add(0, new RowDefinition(GridLength.Auto));
            _columns.Add(0, new ColumnDefinition(GridLength.Auto));

            grid.RowDefinitions.Add(_rows[0]);
            grid.ColumnDefinitions.Add(_columns[0]);

            TextBlock tipTextBlock = new TextBlock
            {
                Text = tip,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            grid.Children.Add(tipTextBlock);
            Grid.SetRow(tipTextBlock, 0);
            Grid.SetColumn(tipTextBlock, 0);
        }
    }
}