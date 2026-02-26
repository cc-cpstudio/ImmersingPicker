using ImmersingPicker.Core;
using ImmersingPicker.Core.Abstractions.Storage;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Helper;
using System.IO;
using System.Text.Json;

namespace ImmersingPicker.Services.Services.Storage;

public class ClassStorageService : IClazzStorageService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public void SaveClasses(List<Clazz> classes)
    {
        var storableClasses = new StorableClasses
        {
            Classes = Clazz.Classes,
            CurrentClassIndex = Clazz.CurrentClassIndex
        };
        string jsonContent = JsonSerializer.Serialize(storableClasses, _jsonOptions);
        File.WriteAllText(ApplicationDataDirPathGetter.GetClassesFilePath(), jsonContent);
    }

    public void LoadClasses()
    {
        string jsonContent = File.ReadAllText(ApplicationDataDirPathGetter.GetClassesFilePath());
        StorableClasses deserialized = JsonSerializer.Deserialize<StorableClasses>(jsonContent, _jsonOptions);
        Clazz.Classes = deserialized.Classes;
        Clazz.CurrentClassIndex = deserialized.CurrentClassIndex;
    }
}