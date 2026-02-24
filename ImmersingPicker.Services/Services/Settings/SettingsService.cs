using System.Text.Json;
using ImmersingPicker.Services.Helper;

namespace ImmersingPicker.Services.Services.Settings;

public class SettingsService
{
    private static readonly Lazy<SettingsService> _currentInstance = new(() => LoadInternal());

    public static SettingsService Current => _currentInstance.Value;

    public PickerSettingsGroup PickerSettingsGroup { get; } = new();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        IgnoreNullValues = false
    };

    private SettingsService() { }

    private static SettingsService LoadInternal()
    {
        try
        {
            if (!File.Exists(ApplicationDataDirPathGetter.GetSettingsFilePath()))
                return new SettingsService();

            string jsonContent = File.ReadAllText(ApplicationDataDirPathGetter.GetSettingsFilePath());
            var settings = JsonSerializer.Deserialize<SettingsService>(jsonContent, _jsonOptions) ??
                           new SettingsService();
            return settings;
        }
        catch (Exception ex)
        {
            return new SettingsService();
        }
    }

    public static void Save() => Current.SaveInternal();

    public static Task SaveAsync() => Current.SaveInternalAsync();

    private void SaveInternal()
    {
        try
        {
            string jsonContent = JsonSerializer.Serialize(this, _jsonOptions);
            File.WriteAllText(ApplicationDataDirPathGetter.GetSettingsFilePath(), jsonContent);
        }
        catch (Exception ex)
        {

        }
    }

    private async Task SaveInternalAsync()
    {
        try
        {
            using var stream = new FileStream(ApplicationDataDirPathGetter.GetSettingsFilePath(), FileMode.Create,
                FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, this, _jsonOptions);
        }
        catch (Exception ex)
        {

        }
    }
}