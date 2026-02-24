using ImmersingPicker.Core;
using ImmersingPicker.Core.Abstractions.Storage;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Helper;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ImmersingPicker.Services.Services.Storage;

public class ClassStorageService : IClazzStorageService
{
    public void SaveClasses(List<Clazz> classes)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        string yamlContent = serializer.Serialize(new StorableClasses
        {
            Classes = Clazz.Classes,
            CurrentClassIndex = Clazz.CurrentClassIndex
        });
        File.WriteAllText(ApplicationDataDirPathGetter.GetClassesFilePath(), yamlContent);
    }

    public void LoadClasses()
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
        StorableClasses deserialized = deserializer.Deserialize<StorableClasses>(ApplicationDataDirPathGetter.GetClassesFilePath());
        Clazz.Classes = deserialized.Classes;
        Clazz.CurrentClassIndex = deserialized.CurrentClassIndex;
    }
}