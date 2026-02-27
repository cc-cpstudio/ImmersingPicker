using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using ImmersingPicker.Controls;
using ImmersingPicker.Core;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Views.MainPages;

public partial class HomePage : UserControl
{
    private int _amountForPicking;

    private Clazz? _clazz;

    private int AmountForPicking
    {
        get => _amountForPicking;
        set
        {
            try
            {
                if (_clazz == null) throw new NullReferenceException();
                if (value > 0 && (_clazz.Students.Count <= 0 || value <= _clazz.Students.Count))
                {
                    _amountForPicking = value;
                    PickButton.Content = $"共{_amountForPicking}人  开始抽选！";
                }
            }
            catch (NullReferenceException)
            {
                _amountForPicking = 1;
                PickButton.Content = $"共{_amountForPicking}人  开始抽选！";
            }
        }
    }

    public HomePage()
    {
        InitializeComponent();
        Clazz.CurrentClassChanged += Reset;
        ClazzComboBox.SelectionChanged += ClazzComboBox_OnSelectionChanged;
        Reset();
    }

    private void ClazzComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is Clazz selectedClazz)
        {
            int index = Clazz.Classes.IndexOf(selectedClazz);
            if (index != -1)
            {
                Clazz.CurrentClassIndex = index;
            }
        }
    }

    private void MinusButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AmountForPicking -= 1;
    }

    private async void PickButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Clazz? currentClazz = Clazz.GetCurrentClazz();
        if (currentClazz == null) return;

        List<Student> picked = currentClazz.Pickers["FairStudentPicker"].Pick(AmountForPicking).Students;

        for (int i = 0; i < 10; i++)
        {
            Seats.DeselectAll();
            foreach (Student student in currentClazz.Pickers["PlainStudentPicker"].Pick(AmountForPicking).Students)
            {
                Seats.Select(student);
            }

            await Task.Delay(100);
        }

        Seats.DeselectAll();
        string dialogContent = "";
        foreach (Student student in picked)
        {
            Seats.Select(student);
            dialogContent += $"{student.Id} {student.Name}\n";
        }

        var dialog = new ContentDialog
        {
            Title = "抽选结果  恭喜以下幸运儿：",
            Content = dialogContent,
            CloseButtonText = "确定"
        };
        await dialog.ShowAsync();
    }

    private void PlusButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AmountForPicking += 1;
    }

    private void ClearButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Seats.DeselectAll();
    }

    public void Reset()
    {
        _clazz = Clazz.GetCurrentClazz();
        _amountForPicking = 1;
        PickButton.Content = $"共{_amountForPicking}人  开始抽选！";
        
        // 更新班级下拉框
        ClazzComboBox.Items.Clear();
        foreach (var clazz in Clazz.Classes)
        {
            ClazzComboBox.Items.Add(clazz);
        }
        
        // 选择当前班级
        if (_clazz != null)
        {
            ClazzComboBox.SelectedItem = _clazz;
        }
    }
}