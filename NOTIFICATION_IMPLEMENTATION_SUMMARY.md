# 系统通知功能实现总结

## ✅ 已完成的功能

### 1. 系统通知服务
**文件**: `ImmersingPicker.Services/Services/SystemNotificationService.cs`

**功能**:
- ✅ 使用 Windows Toast API 发送系统级通知
- ✅ 使用 PowerShell 脚本调用 Windows Runtime
- ✅ 降级机制：如果 Toast 失败，显示消息框
- ✅ 文本截断功能，避免通知过长

**提供的方法**:
- `ShowNotification(title, message)` - 发送通用通知
- `ShowUpdateAvailableNotification(version, releaseNotes)` - 发送更新可用通知
- `ShowUpdateDownloadedNotification()` - 发送更新下载完成通知
- `ShowUpdateFailedNotification(errorMessage)` - 发送更新失败通知

### 2. 集成到启动检查更新
**文件**: `ImmersingPicker/App.axaml.cs`

**修改内容**:
- 在 `StartUpdateCheckOnStartup()` 方法中添加通知功能
- 当主窗口不在前台时，发送系统通知提醒用户有新版本
- 通知后会显示更新对话框

**代码位置**:
```csharp
// 如果主窗口不在前台，发送系统通知
if (!_isMainWindowActive)
{
    SystemNotificationService.Instance.ShowUpdateAvailableNotification(
        updateInfo.Version,
        updateInfo.ReleaseNotes);
}
```

### 3. 集成到手动检查更新
**文件**: `ImmersingPicker/Views/SettingsPages/UpdateSettingsPage.axaml.cs`

**修改内容**:
- 在 `ShowUpdateDialogAsync()` 方法中添加通知功能
- 当发现新版本时，先发送系统通知，再显示更新对话框

**代码位置**:
```csharp
// 发送系统通知
SystemNotificationService.Instance.ShowUpdateAvailableNotification(
    updateInfo.Version,
    updateInfo.ReleaseNotes);
```

### 4. 集成到更新对话框
**文件**: `ImmersingPicker/Views/Dialogs/UpdateDialog.axaml.cs`

**修改内容**:
- 在更新完成后发送系统通知
- 在更新失败时发送系统通知

**代码位置**:
```csharp
// 更新完成时
SystemNotificationService.Instance.ShowUpdateDownloadedNotification();

// 更新失败时
SystemNotificationService.Instance.ShowUpdateFailedNotification("安装更新失败");
```

## 🎯 通知触发时机

| 场景 | 触发条件 | 通知内容 |
|------|---------|---------|
| **启动检查** | 应用启动时发现新版本且窗口不在前台 | "发现新版本 - ImmersingPicker X.X.X 已发布!" |
| **手动检查** | 用户在设置页面手动检查更新 | "发现新版本 - ImmersingPicker X.X.X 已发布!" |
| **下载完成** | 更新文件下载完成 | "更新已下载完成 - ImmersingPicker 更新已下载完成,准备安装。" |
| **更新失败** | 安装更新失败 | "更新失败 - ImmersingPicker 更新失败: [错误信息]" |

## 🔧 技术实现

### Windows Toast 通知
使用 PowerShell 脚本调用 Windows Runtime API:

```powershell
# 加载 Windows Runtime
[Windows.UI.Notifications.ToastNotificationManager, Windows.UI.Notifications, ContentType = WindowsRuntime] | Out-Null

# 获取模板
$toastTemplate = [Windows.UI.Notifications.ToastTemplateType]::ToastText02
$toastXml = [Windows.UI.Notifications.ToastNotificationManager]::GetTemplateContent($toastTemplate)

# 设置文本
$textElements = $toastXml.GetElementsByTagName('text')
$textElements.Item(0).AppendChild($toastXml.CreateTextNode('标题')) | Out-Null
$textElements.Item(1).AppendChild($toastXml.CreateTextNode('内容')) | Out-Null

# 创建并显示 Toast
$xmlDoc = New-Object Windows.Data.Xml.Dom.XmlDocument
$xmlDoc.LoadXml($toastXml.GetXml())
$toast = New-Object Windows.UI.Notifications.ToastNotification($xmlDoc)
[Windows.UI.Notifications.ToastNotificationManager]::CreateToastNotifier('ImmersingPicker').Show($toast)
```

### 降级机制
如果 Toast 通知失败，使用 PowerShell 显示消息框作为降级方案:

```powershell
Add-Type -AssemblyName PresentationFramework
[System.Windows.MessageBox]::Show('内容', '标题', 'OK', 'Information')
```

## 📋 通知示例

### 更新可用通知
```
标题: 发现新版本
内容: ImmersingPicker 0.2.0.3 已发布!

修复了设置页面显示问题...

打开设置检查更新
```

### 下载完成通知
```
标题: 更新已下载完成
内容: ImmersingPicker 更新已下载完成,准备安装。
```

### 更新失败通知
```
标题: 更新失败
内容: ImmersingPicker 更新失败: 安装程序退出码: 1
```

## ⚠️ 注意事项

1. **Windows 10/11 支持**: Toast 通知需要 Windows 10 或更高版本
2. **权限**: 不需要特殊权限即可发送通知
3. **超时**: 通知会在 5 分钟后自动消失
4. **应用标识**: 所有通知都使用 "ImmersingPicker" 作为应用标识
5. **错误处理**: 如果 Toast 失败，会自动降级为消息框

## 🎉 实现完成!

现在当发现新版本时，系统会发送通知提醒用户，包括：
- ✅ 启动时自动检查并通知
- ✅ 手动检查时通知
- ✅ 下载完成时通知
- ✅ 更新失败时通知

用户即使在使用其他应用或不注意主窗口时，也能通过系统通知了解到更新信息!
