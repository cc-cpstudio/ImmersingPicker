using ImmersingPicker.Core.Abstractions.Picker;

namespace ImmersingPicker.Core.Models;

public class Clazz
{
    protected void OnStudentListChanged()
    {
        StudentListChanged?.Invoke();
    }

    protected static void OnCurrentClassChanged()
    {
        CurrentClassChanged?.Invoke();
    }

    public event Action StudentListChanged;
    public static event Action CurrentClassChanged;

    private static int _currentClassIndex = -1;

    public string Name { get; set; }
    public List<Student> Students { get; set; }
    public List<History> Histories { get; set; }
    public Dictionary<string, PickerBase> Pickers { get; set; }

    public static List<Clazz> Classes { get; set; } = new();

    public static int CurrentClassIndex
    {
        get => _currentClassIndex;
        set
        {
            _currentClassIndex = value;
            OnCurrentClassChanged();
        }
    }

    static Clazz()
    {
        Classes =
        [
            new Clazz("Hello world!",
                [
                    new Student("s1",
                        1,
                        1,
                        1),
                    new Student("s2",
                        2,
                        1,
                        2),
                    new Student("s3",
                        3,
                        2,
                        1),
                    new Student("s4",
                        4,
                        2,
                        2),
                    new Student("s5",
                        5,
                        3,
                        1),
                    new Student("s6",
                        6,
                        3,
                        2),
                    new Student("s7",
                        7,
                        4,
                        1),
                    new Student("s8",
                        8,
                        4,
                        2),
                    new Student("s9",
                        9,
                        5,
                        1),
                    new Student("s10",
                        10,
                        5,
                        2),
                ],
            [])];
        Classes = new List<Clazz>();
        _currentClassIndex = 0;
    }

    public static Clazz? GetCurrentClazz()
    {
        return GetClazz(CurrentClassIndex);
    }

    public static Clazz? GetClazz(int index)
    {
        if (index < 0 || index >= Classes.Count) return null;
        return Classes[index];

    }

    public static bool CheckIfNameExists(string name)
    {
        return Classes.Any(clazz => clazz.Name == name);
    }

    public Clazz()
    {
        Name = "Nameless-Clazz";
        Students = new List<Student>();
        Histories = new List<History>();
        Pickers = new Dictionary<string, PickerBase>();
    }

    public Clazz(string name)
    {
        Name = name;
        Students = new List<Student>();
        Histories = new List<History>();
        Pickers = new Dictionary<string, PickerBase>();

        if (Classes != null)
        {
            Classes.Add(this);
        }
    }

    public Clazz(string name, List<Student> students, List<History> histories)
    {
        Name = name;
        Students = students;
        Histories = histories;
        Pickers = new Dictionary<string, PickerBase>();

        if (Classes != null)
        {
            Classes.Add(this);
        }
    }





    public bool CheckIfIdExists(int id)
    {
        bool flag = false;
        foreach (Student student in Students)
        {
            if (student.Id == id)
            {
                flag = true;
                break;
            }
        }

        return flag;
    }

    public Student? FindStudentById(int id)
    {
        return Students.Find(s => s.Id == id);
    }

    public void AddStudent(string name, int id, ValueTuple<int, int> seat)
    {
        if (!CheckIfIdExists(id))
        {
            Student s = new Student(name, id, seat.Item1, seat.Item2);
            Students.Add(s);
            OnStudentListChanged();
        }
    }

    public void SetStudentInitialWeight(int id, double initialWeight)
    {
        if (CheckIfIdExists(id))
        {
            FindStudentById(id)!.InitialWeight = initialWeight;
        }
    }

    public void RemoveStudent(int id)
    {
        if (CheckIfIdExists(id))
        {
            Students.Remove(Students.First(s => s.Id == id));
            OnStudentListChanged();
        }
    }

    public History? Pick(string picker, int amount)
    {
        if (!Pickers.ContainsKey(picker)) return null;
        History history = Pickers[picker].Pick(amount);
        Histories.Add(history);
        return history;
    }
}