namespace ImmersingPicker.Core;

public class Student
{
    public string Name { get; init; }
    public int Id { get; init; }
    public int SeatRow { get; set; }
    public int SeatColumn { get; set; }
    public double InitialWeight { get; set; }
    public DateTime? LastSelectedTime { get; set; }
    public int SelectedAmount { get; set; } = 0;
    public double Weight { get; set; }

    public Student(string name, int id, int seatRow, int seatColumn)
    {
        if (id <= 0)
        {
            // TODO 抛出异常
        }
        else
        {
            Name = name;
            Id = id;
            SeatRow = seatRow;
            SeatColumn = seatColumn;
        }
    }

    public void resetHistories()
    {
        LastSelectedTime = null;
        SelectedAmount = 0;
        Weight = -1.0;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not Student other) return false;
        return Id == other.Id && string.Equals(Name, other.Name);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }

    public override string ToString()
    {
        return $"Student(id={Id},name={Name})";
    }

    public static bool operator ==(Student? left, Student? right)
    {
        if (left is null || right is null) return false;
        return left.Id == right.Id && left.Name == right.Name;
    }

    public static bool operator !=(Student? left, Student? right)
    {
        return !(left == right);
    }
}