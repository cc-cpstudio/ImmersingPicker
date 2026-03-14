using ImmersingPicker.Core.Abstractions.Picker;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Services.Services.Picker;

public static class ClazzFactory
{
    public static Clazz NewClazz(string name, List<Student>? students = null, List<History>? histories = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("班级名称不能为空", nameof(name));
        }

        var newClazz = new Clazz(
            name,
            students ?? new List<Student>(),
            histories ?? new List<History>()
        );

        _ = new FairStudentPicker(newClazz);
        _ = new PlainStudentPicker(newClazz);

        if (Clazz.Classes.Count == 1)
        {
            Clazz.CurrentClassIndex = 0;
        }

        return newClazz;
    }
}
