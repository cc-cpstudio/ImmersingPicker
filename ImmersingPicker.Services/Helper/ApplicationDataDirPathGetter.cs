namespace ImmersingPicker.Services.Helper;

public class ApplicationDataDirPathGetter
{
    public static string Get()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ImmersingPicker", "v0.0.0.a");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    public static string GetClassesFilePath()
    {
        return Path.Combine(Get(), "Classes.yml");
    }

    public static string GetSettingsFilePath()
    {
        return Path.Combine(Get(), "Settings.json");
    }
}