using ImmersingPicker.Core;
using ImmersingPicker.Core.Abstractions.Picker;
using ImmersingPicker.Core.Exceptions;
using ImmersingPicker.Core.Models;

namespace ImmersingPicker.Services.Services.Picker;

/// <summary>
/// 公平学生选择器，基于权重计算实现公平抽取
/// </summary>
public class FairStudentPicker(Clazz clazz) : PickerBase(clazz)
{
    /// <summary>
    /// 选择器名称
    /// </summary>
    public override string Name { get; set; } = "FairStudentPicker";
    
    /// <summary>
    /// 是否需要存储历史记录
    /// </summary>
    public override bool NeedStore { get; set; } = true;

    /// <summary>
    /// 计算学生选中次数的范围
    /// </summary>
    /// <param name="students">学生列表</param>
    /// <returns>选中次数的范围（最大值 - 最小值）</returns>
    /// <exception cref="NoAvailableStudentException">当学生列表为空时抛出</exception>
    private int CalculateRange(List<Student> students)
    {
        if (students.Count == 0)
        {
            throw new NoAvailableStudentException();
        }
        
        Student maxSelectedStudent = students.MaxBy(s => s.SelectedAmount)
            ?? throw new NoAvailableStudentException();
        Student minSelectedStudent = students.MinBy(s => s.SelectedAmount)
            ?? throw new NoAvailableStudentException();
        
        return maxSelectedStudent.SelectedAmount - minSelectedStudent.SelectedAmount;
    }

    /// <summary>
    /// 计算可用于抽取的学生列表
    /// </summary>
    /// <returns>可用于抽取的学生列表</returns>
    private List<Student> CalculateAvailablePickStudents()
    {
        // 创建学生列表的副本，避免修改原始列表
        List<Student> availableStudents = new List<Student>(_clazz.Students);
        
        // 确保至少有一个学生
        if (availableStudents.Count == 0)
        {
            return availableStudents;
        }
        
        // 计算选中次数的范围
        int selectionRange = CalculateRange(availableStudents);
        
        // 移除选中次数过多的学生，直到范围小于等于1或只剩一个学生
        while (selectionRange > 1 && availableStudents.Count > 1)
        {
            var maxSelectedStudent = availableStudents.MaxBy(s => s.SelectedAmount);
            if (maxSelectedStudent != null)
            {
                availableStudents.Remove(maxSelectedStudent);
                selectionRange = CalculateRange(availableStudents);
            }
            else
            {
                break;
            }
        }

        // 计算所有学生的平均选中次数
        double averageSelectionCount = _clazz.Students.Sum(s => s.SelectedAmount) / Convert.ToDouble(_clazz.Students.Count);
        
        // 移除选中次数高于平均值的学生
        availableStudents.RemoveAll(s => s.SelectedAmount > averageSelectionCount);
        
        // 如果筛选后为空，返回选中次数最少的学生
        if (availableStudents.Count == 0)
        {
            var minSelectedStudent = _clazz.Students.MinBy(s => s.SelectedAmount);
            if (minSelectedStudent != null)
            {
                availableStudents.Add(minSelectedStudent);
            }
        }
        
        return availableStudents;
    }

    /// <summary>
    /// 计算每个学生的权重
    /// </summary>
    private void CalculateWeight()
    {
        // 计算所有学生的平均选中次数
        double averageSelectionCount = _clazz.Students.Sum(s => s.SelectedAmount) / Convert.ToDouble(_clazz.Students.Count);
        
        foreach (Student student in _clazz.Students)
        {
            // 重置权重为初始权重
            student.Weight = student.InitialWeight;
            
            // 基于选中次数的权重调整
            // 选中次数越少，权重越高
            double selectionWeight = averageSelectionCount - student.SelectedAmount + 1; // +1 确保权重为正
            student.Weight += selectionWeight;
            
            // 基于时间间隔的权重调整
            if (student.LastSelectedTime != null)
            {
                TimeSpan timeSinceLastSelection = DateTime.Now - student.LastSelectedTime.Value;
                int daysSinceLastSelection = timeSinceLastSelection.Days;
                student.Weight += daysSinceLastSelection > 0 ? daysSinceLastSelection : 1;
            }
            else
            {
                student.Weight += 5; // 从未被选中的学生给予较高权重
            }

            student.Weight *= 4;
            
            // 随机因子，增加随机性
            student.Weight += _random.NextDouble()*3;
        }
    }

    /// <summary>
    /// 执行抽取逻辑
    /// </summary>
    /// <param name="amount">要抽取的学生数量</param>
    /// <returns>抽取结果</returns>
    protected override History PickLogic(int amount)
    {
        // 计算每个学生的权重
        CalculateWeight();
        
        // 创建优先队列，按权重降序排列
        PriorityQueue<Student, double> priorityQueue = new(
            Comparer<double>.Create((x, y) => y.CompareTo(x))
        );
        
        // 将所有学生加入优先队列
        foreach (Student student in _clazz.Students)
        {
            priorityQueue.Enqueue(student, student.Weight);
        }

        // 抽取指定数量的学生
        List<Student> pickedStudents = new();
        for (int i = 0; i < amount && priorityQueue.Count > 0; i++)
        {
            pickedStudents.Add(priorityQueue.Dequeue());
        }

        // 按学生ID排序
        pickedStudents.Sort((s1, s2) => s1.Id.CompareTo(s2.Id));

        // 创建历史记录
        return new History(DateTime.Now, Name, pickedStudents);
    }
}