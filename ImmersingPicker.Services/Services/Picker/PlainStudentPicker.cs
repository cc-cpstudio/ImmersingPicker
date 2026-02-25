using ImmersingPicker.Core;
using ImmersingPicker.Core.Abstractions.Picker;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Services.Services.Picker;

public class PlainStudentPicker(Clazz clazz) : PickerBase(clazz)
{
    public override string Name { get; set; } = "PlainStudentPicker";
    public override bool NeedStore { get; set; } = false;

    private List<Student> Shuffle(List<Student> students)
    {
        if (students.Count == 1) return students;
        List<Student> tmp = new List<Student>(students);
        int n = tmp.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            (tmp[n], tmp[k]) = (tmp[k], tmp[n]);
        }

        return tmp;
    }

    protected override History PickLogic(int amount)
    {
        List<Student> shuffled = Shuffle(_clazz.Students), result = new List<Student>();
        for (int i = 0; i < amount; i++)
        {
            result.Add(shuffled[i]);
        }
        return new History(DateTime.Now, Name, result);
    }
}