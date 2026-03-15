using Avalonia.Controls;
using ImmersingPicker.Services.Services;
using ImmersingPicker.Services.Services.Security;

namespace ImmersingPicker.Controls;

public partial class PasswordVerifyDialog : UserControl
{
    private readonly PasswordService _passwordService = PasswordService.Instance;

    public PasswordVerifyDialog()
    {
        InitializeComponent();
    }

    public bool Verify()
    {
        string password = PasswordBox.Text ?? string.Empty;

        if (string.IsNullOrEmpty(password))
        {
            ShowError("请输入密码");
            return false;
        }

        if (!_passwordService.VerifyPassword(password))
        {
            ShowError("密码错误");
            return false;
        }

        return true;
    }

    private void ShowError(string message)
    {
        ErrorTextBlock.Text = message;
        ErrorTextBlock.IsVisible = true;
    }

    public void Clear()
    {
        PasswordBox.Text = string.Empty;
        ErrorTextBlock.IsVisible = false;
        ErrorTextBlock.Text = string.Empty;
    }
}
