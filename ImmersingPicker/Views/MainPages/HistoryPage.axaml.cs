using Avalonia.Controls;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Views.MainPages;

public partial class HistoryPage : UserControl
{
    private Clazz? _clazz;

    public HistoryPage()
    {
        InitializeComponent();
        _clazz = Clazz.GetCurrentClazz();
        if (_clazz != null)
        {
            TitleText.Text = $"班级 {_clazz.Name} 的历史记录";
        }
    }

    public void RefreshClazz()
    {
        _clazz = Clazz.GetCurrentClazz();
        if (_clazz == null) return;
        TitleText.Text = $"班级 {_clazz.Name} 的历史记录";
    }
}