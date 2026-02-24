namespace ImmersingPicker.Core;

public class Clazz
{
    public string Name { get; set; }
    public List<Student> Students { get; set; }
    public List<History> Histories { get; set; }

    public static List<Clazz> Classes { get; set; }
    public static int CurrentClassIndex { get; set; }

    public static Clazz? GetCurrentClazz()
    {
        return GetClazz(CurrentClassIndex);
    }

    public static Clazz? GetClazz(int index)
    {
        if (index >= 0 && index < Classes.Count)
        {
            return Classes[index];
        }

        return null;
    }

    public Clazz(string name)
    {
        Name = name;
        Students = new List<Student>();
        Histories = new List<History>();

        Classes.Add(this);
    }

    public Clazz(string name, List<Student> students, List<History> histories)
    {
        Name = name;
        Students = students;
        Histories = histories;

        Classes.Add(this);
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

    public void AddStudent(string name, int id, ValueTuple<int, int> seat)
    {
        if (!CheckIfIdExists(id))
        {
            Student s = new Student(name, id, seat.Item1, seat.Item2);
            Students.Add(s);
        }
        else
        {

        }
    }

    public void RemoveStudent(int id)
    {
        if (CheckIfIdExists(id))
        {
            Students.Remove(Students.First(s => s.Id == id));
        }
        else
        {

        }
    }
}