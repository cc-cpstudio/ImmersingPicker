using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ImmersingPicker.Core;

namespace ImmersingPicker.Controls;

public partial class Seat : UserControl
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Seat, string>(nameof(Text), defaultValue: "未知学生");

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Student Student { get; }

    public Seat()
    {
        InitializeComponent();
        DataContext = this;
    }

    public Seat(Student student)
    {
        InitializeComponent();
        DataContext = this;
        Student = student;
        Text = $"{Student.Id} {Student.Name}";
        TextBlock.Text = Text;
    }
}