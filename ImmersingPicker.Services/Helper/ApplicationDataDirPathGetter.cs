using System;
using System.IO;

namespace ImmersingPicker.Services.Helper;

public class ApplicationDataDirPathGetter
{
    public static string Get()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ipicker");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    public static string GetClassesFilePath()
    {
        return Path.Combine(Get(), "Classes.json");
    }

    public static string GetSettingsFilePath()
    {
        return Path.Combine(Get(), "Settings.json");
    }

    public static string GetLogFilePath()
    {
        return Path.Combine(Get(), "Logs\\");
    }
}