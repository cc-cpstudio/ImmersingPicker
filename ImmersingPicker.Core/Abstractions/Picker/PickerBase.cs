namespace ImmersingPicker.Core.Abstractions.Picker;

public abstract class PickerBase(Clazz clazz)
{
    public abstract string Name { get; set; }

    protected readonly Random _random = new();

    protected Clazz _clazz = clazz;

    public abstract History PickLogic(int amount);

    public History Pick(int amount)
    {
        History history = PickLogic(amount);
        foreach (Student student in history.Students)
        {
            student.LastSelectedTime = history.CreateTime;
            student.SelectedAmount ++;
        }

        return history;
    }
}