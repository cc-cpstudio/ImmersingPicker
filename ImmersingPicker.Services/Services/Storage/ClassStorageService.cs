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
        // 创建一个临时列表，用于存储不包含Pickers的班级对象
        var classesToSave = new List<Clazz>();
        foreach (var clazz in Clazz.Classes)
        {
            var classToSave = new Clazz
            {
                Name = clazz.Name,
                Students = clazz.Students,
                Histories = clazz.Histories
            };
            classesToSave.Add(classToSave);
        }
        
        var storableClasses = new StorableClasses
        {
            Classes = classesToSave,
            CurrentClassIndex = Clazz.CurrentClassIndex
        };
        string jsonContent = JsonSerializer.Serialize(storableClasses, _jsonOptions);
        File.WriteAllText(ApplicationDataDirPathGetter.GetClassesFilePath(), jsonContent);
    }

    public void LoadClasses()
    {
        string filePath = ApplicationDataDirPathGetter.GetClassesFilePath();
        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            StorableClasses deserialized = JsonSerializer.Deserialize<StorableClasses>(jsonContent, _jsonOptions);
            
            // 清空现有班级列表
            Clazz.Classes.Clear();
            
            // 重新创建班级对象，并为每个班级添加Picker
            foreach (var clazz in deserialized.Classes)
            {
                var newClazz = new Clazz(
                    clazz.Name, 
                    clazz.Students, 
                    clazz.Histories
                );
                // 为新班级添加Picker
                new ImmersingPicker.Services.Services.Picker.FairStudentPicker(newClazz);
                new ImmersingPicker.Services.Services.Picker.PlainStudentPicker(newClazz);
            }
            
            Clazz.CurrentClassIndex = deserialized.CurrentClassIndex;
        }
    }
}