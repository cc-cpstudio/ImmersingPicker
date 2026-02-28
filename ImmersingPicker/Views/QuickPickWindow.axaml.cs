using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using ImmersingPicker.Core;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Views;

public partial class QuickPickWindow : Window
{
    public QuickPickWindow()
    {
        InitializeComponent();
        Loaded += QuickPickWindow_Loaded;
    }

    private async void QuickPickWindow_Loaded(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        await PerformQuickPick();
    }

    private async Task PerformQuickPick()
    {
        Clazz? currentClazz = Clazz.GetCurrentClazz();
        if (currentClazz == null) return;

        // 执行抽选动画
        List<Student> picked = await Seats.PickAsync(1);

        // 显示抽选结果
        if (picked.Count > 0)
        {
            Student student = picked[0];
            string dialogContent = $"{student.Id} {student.Name}";

            var dialog = new ContentDialog
            {
                Title = "抽选结果  恭喜以下幸运儿：",
                Content = dialogContent,
                CloseButtonText = "确定"
            };

            await dialog.ShowAsync();

            // 等待两秒后关闭窗口
            await Task.Delay(2000);
            Close();
        }
    }
}
