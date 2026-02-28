using ImmersingPicker.Core;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Services.Picker;

namespace ImmersingPicker.Test;

public class FairnessTests
{
    [Fact]
    public void TestFairnessOverMultipleSelections()
    {
        // 准备测试数据
        var students = new List<Student>
        {
            new Student("学生1", 1, 1, 1),
            new Student("学生2", 2, 1, 2),
            new Student("学生3", 3, 2, 1),
            new Student("学生4", 4, 2, 2)
        };
        var clazz = new Clazz("测试班级");
        clazz.Students.AddRange(students);

        // 创建FairStudentPicker
        var picker = new FairStudentPicker(clazz);

        // 执行多次抽取
        const int totalSelections = 100;
        const int studentsPerSelection = 2;
        var selectionCounts = new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 }
        };

        for (int i = 0; i < totalSelections; i++)
        {
            var result = picker.Pick(studentsPerSelection);
            foreach (var student in result.Students)
            {
                selectionCounts[student.Id]++;
            }
        }

        // 验证每个学生的选中次数是否相对公平
        // 这里我们简单验证没有学生被选中次数过多或过少
        int expectedSelections = (totalSelections * studentsPerSelection) / students.Count;
        int tolerance = expectedSelections / 2; // 允许一定的偏差

        foreach (var (studentId, count) in selectionCounts)
        {
            Assert.InRange(count, expectedSelections - tolerance, expectedSelections + tolerance);
        }
    }

    [Fact]
    public void TestTimeWeightEffect()
    {
        // 准备测试数据
        var students = new List<Student>
        {
            new Student("学生1", 1, 1, 1),
            new Student("学生2", 2, 1, 2)
        };
        var clazz = new Clazz("测试班级");
        clazz.Students.AddRange(students);

        // 设置不同的最后选中时间
        students[0].LastSelectedTime = DateTime.Now.AddDays(-5); // 5天前
        students[1].LastSelectedTime = DateTime.Now.AddDays(-1); // 1天前

        // 创建FairStudentPicker
        var picker = new FairStudentPicker(clazz);

        // 执行多次抽取
        const int totalSelections = 20;
        var selectionCounts = new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 0 }
        };

        for (int i = 0; i < totalSelections; i++)
        {
            var result = picker.Pick(1);
            selectionCounts[result.Students[0].Id]++;
        }

        // 验证长时间未被选中的学生被选中的次数更多
        Assert.True(selectionCounts[1] > selectionCounts[2]);
    }

    [Fact]
    public void TestSelectionCountWeightEffect()
    {
        // 准备测试数据
        var students = new List<Student>
        {
            new Student("学生1", 1, 1, 1),
            new Student("学生2", 2, 1, 2)
        };
        var clazz = new Clazz("测试班级");
        clazz.Students.AddRange(students);

        // 设置不同的选中次数
        students[0].SelectedAmount = 5; // 被选中5次
        students[1].SelectedAmount = 1; // 被选中1次

        // 创建FairStudentPicker
        var picker = new FairStudentPicker(clazz);

        // 执行多次抽取
        const int totalSelections = 20;
        var selectionCounts = new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 0 }
        };

        for (int i = 0; i < totalSelections; i++)
        {
            var result = picker.Pick(1);
            selectionCounts[result.Students[0].Id]++;
        }

        // 验证选中次数少的学生被选中的次数更多
        Assert.True(selectionCounts[2] > selectionCounts[1]);
    }
}
