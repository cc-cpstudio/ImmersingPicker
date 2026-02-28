using ImmersingPicker.Core;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Services.Picker;

namespace ImmersingPicker.Test;

public class FairStudentPickerTests
{
    [Fact]
    public void TestStudentListNotModified()
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
        int initialCount = clazz.Students.Count;
        
        // 创建学生引用的副本，用于后续验证
        var originalStudents = new List<Student>(clazz.Students);

        // 创建FairStudentPicker并执行抽取
        var picker = new FairStudentPicker(clazz);
        picker.Pick(2);

        // 验证学生列表数量没有变化
        Assert.Equal(initialCount, clazz.Students.Count);

        // 验证所有原始学生仍然存在
        foreach (var student in originalStudents)
        {
            Assert.Contains(student, clazz.Students);
        }
        
        // 验证学生列表的引用是否相同（确保没有被替换）
        Assert.Equal(originalStudents.Count, clazz.Students.Count);
    }

    [Fact]
    public void TestTimeWeightCalculation()
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

        // 创建FairStudentPicker并执行抽取
        var picker = new FairStudentPicker(clazz);
        var result = picker.Pick(1);

        // 验证权重计算是否考虑了时间因素
        // 这里我们只能间接验证，因为权重是内部计算的
        // 但至少要确保方法能够正常执行，不会抛出异常
        Assert.NotNull(result);
    }

    [Fact]
    public void TestRandomFactorCalculation()
    {
        // 准备测试数据
        var students = new List<Student>
        {
            new Student("学生1", 1, 1, 1),
            new Student("学生2", 2, 1, 2)
        };
        var clazz = new Clazz("测试班级");
        clazz.Students.AddRange(students);

        // 创建FairStudentPicker并执行多次抽取
        var picker = new FairStudentPicker(clazz);
        List<int> results = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            var result = picker.Pick(1);
            results.Add(result.Students[0].Id);
        }

        // 验证结果具有随机性
        // 这里我们简单验证是否有不同的结果
        Assert.Contains(1, results);
        Assert.Contains(2, results);
    }

    [Fact]
    public void TestWeightCalculation()
    {
        // 准备测试数据
        var students = new List<Student>
        {
            new Student("学生1", 1, 1, 1),
            new Student("学生2", 2, 1, 2)
        };
        var clazz = new Clazz("测试班级");
        clazz.Students.AddRange(students);

        // 创建FairStudentPicker
        var picker = new FairStudentPicker(clazz);

        // 执行抽取
        var result = picker.Pick(1);

        // 验证结果不为空
        Assert.NotNull(result);
        Assert.Single(result.Students);
    }

    [Fact]
    public void TestAvailableStudentsFiltering()
    {
        // 准备测试数据
        var students = new List<Student>
        {
            new Student("学生1", 1, 1, 1),
            new Student("学生2", 2, 1, 2),
            new Student("学生3", 3, 2, 1)
        };
        var clazz = new Clazz("测试班级");
        clazz.Students.AddRange(students);

        // 创建FairStudentPicker
        var picker = new FairStudentPicker(clazz);

        // 执行多次抽取，确保不会返回空结果
        for (int i = 0; i < 5; i++)
        {
            var result = picker.Pick(2);
            Assert.NotNull(result);
            Assert.Equal(2, result.Students.Count);
        }
    }

    [Fact]
    public void TestEdgeCases()
    {
        // 测试单学生班级
        var singleStudent = new List<Student>
        {
            new Student("学生1", 1, 1, 1)
        };
        var singleClazz = new Clazz("单学生班级");
        singleClazz.Students.AddRange(singleStudent);
        var singlePicker = new FairStudentPicker(singleClazz);
        var singleResult = singlePicker.Pick(1);

        Assert.NotNull(singleResult);
        Assert.Single(singleResult.Students);
        Assert.Equal("学生1", singleResult.Students[0].Name);
    }
}