using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ImmersingPicker.Core;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Controls;

public partial class StudentEditTableViewItem : UserControl
{
    public static readonly StyledProperty<Student> StudentProperty = 
        AvaloniaProperty.Register<StudentEditTableViewItem, Student>(nameof(Student));
    
    public Student Student
    {
        get => GetValue(StudentProperty);
        set => SetValue(StudentProperty, value);
    }
    
    public event Action<Student>? DeleteRequested;
    
    public StudentEditTableViewItem()
    {
        InitializeComponent();
        DataContext = this;
    }
    
    private void DeleteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Student != null)
        {
            DeleteRequested?.Invoke(Student);
        }
    }
}