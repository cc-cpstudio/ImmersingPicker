using ImmersingPicker.Core;
using ImmersingPicker.Core.Abstractions;
using ImmersingPicker.Core.Abstractions.Picker;
using ImmersingPicker.Core.Exceptions;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Services.Settings;

namespace ImmersingPicker.Services.Services.Picker;

public class StudentFairPicker(Clazz clazz) : PickerBase(clazz)
{
    public override string Name { get; set; } = "StudentPicker";

    private int CalculateRange(List<Student> needed)
    {
        Student maxE = needed.MaxBy(s => s.SelectedAmount)
            ?? throw new NoAvailableStudentException();
        Student minE = needed.MinBy(s => s.SelectedAmount)
            ?? throw new NoAvailableStudentException();
        return maxE.SelectedAmount - minE.SelectedAmount;
    }

    private List<Student> CalculateAvailablePickStudents()
    {
        List<Student> tmpStudents = _clazz.Students;
        int availableRange = CalculateRange(tmpStudents);
        while (availableRange > SettingsService.Current.PickerSettingsGroup.MaxAvailableRange.Value &&
               tmpStudents.Count >= SettingsService.Current.PickerSettingsGroup.MinSelectionPoolAmount.Value)
        {
            tmpStudents.Remove(tmpStudents.MaxBy(s => s.SelectedAmount));
            availableRange = CalculateRange(tmpStudents);
        }

        double average = _clazz.Students.Sum(s => s.SelectedAmount) / Convert.ToDouble(_clazz.Students.Count);
        tmpStudents.RemoveAll(s => s.SelectedAmount > average);
        return tmpStudents;
    }

    private void CalculateWeight()
    {
        List<Student> available = CalculateAvailablePickStudents();
        double average = _clazz.Students.Sum(s => s.SelectedAmount) / Convert.ToDouble(_clazz.Students.Count);
        foreach (Student student in _clazz.Students)
        {
            student.Weight = student.Weight <= student.InitialWeight ? student.InitialWeight : Math.Log(student.Weight);
        }
        foreach (Student student in _clazz.Students)
        {
            student.Weight += available.Contains(student)
                ? Math.Pow(student.SelectedAmount - average,
                    SettingsService.Current.PickerSettingsGroup.WeighCoefficientOfSelectedAmount.Value)
                : SettingsService.Current.PickerSettingsGroup.WeighCoefficientOfSelectedAmount
                    .Value;

            if (student.LastSelectedTime != null)
            {
                TimeSpan? span = DateTime.Now - student.LastSelectedTime;
                student.Weight += span.GetValueOrDefault().Days > SettingsService.Current.PickerSettingsGroup.IntervalDays.Value
                    ? Math.Pow(SettingsService.Current.PickerSettingsGroup.WeighCoefficientOfSelectedTime.Value, span.GetValueOrDefault().Days)
                    : SettingsService.Current.PickerSettingsGroup.WeighCoefficientOfSelectedTime.Value;
            }
            else
            {
                student.Weight += SettingsService.Current.PickerSettingsGroup.WeighCoefficientOfNullSelectedTime.Value;
            }

            student.Weight += Convert.ToDouble(_random.Next(
                                  SettingsService.Current.PickerSettingsGroup.RandomWeighCoefficientLowerBound.Value,
                                  SettingsService.Current.PickerSettingsGroup.RandomWeighCoefficientUpperBound.Value)) +
                              _random.NextDouble();
        }
    }

    public override History PickLogic(int amount)
    {
        PriorityQueue<Student, double> pq = new(
            Comparer<double>.Create((x, y) => y.CompareTo(x))
        );
        foreach (Student student in _clazz.Students.OrderBy(_ => _random.Next()).ToList())
        {
            pq.Enqueue(student, student.Weight);
        }

        List<Student> picked = new();
        for (int i = 0; i < amount; i++)
        {
            picked.Add(pq.Dequeue());
        }

        picked.Sort((s1, s2) => s1.Id.CompareTo(s2.Id));

        return new History(DateTime.Now, Name, picked);
    }
}