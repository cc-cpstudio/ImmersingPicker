package backend

import com.github.immersingeducation.immersingpicker.backend.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.NO_GENDER
import com.github.immersingeducation.immersingpicker.backend.Student
import org.junit.jupiter.api.Assertions
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.assertThrows

class SelectorTest {
    fun constructStudent(): MutableList<Student> {
        val s1 = Student(name = "s1", id = 1, gender = NO_GENDER, group = "g1", seat = Pair(1, 1))
        val s2 = Student(name = "s2", id = 2, gender = NO_GENDER, group = "g1", seat = Pair(1, 2))
        val s3 = Student(name = "s4", id = 4, gender = NO_GENDER, group = "g2", seat = Pair(1, 3))
        return mutableListOf(s1, s2, s3)
    }

    @Test
    fun `test select by student`() {
        val class1 = ClassNGrade(name = "class1", students = constructStudent())
        Assertions.assertEquals("所需的抽取数量 4 不允许大于班级人数 3", assertThrows<Throwable> {
            class1.studentSelector.select(4)
        }.message)
        Assertions.assertTrue(class1.students.containsAll(class1.studentSelector.select(2)))
    }

    @Test
    fun `test select by group`() {
        val class1 = ClassNGrade(name = "class1", students = constructStudent())
        Assertions.assertEquals("所需的抽取数量 3 不允许大于班级组数 2", assertThrows<Throwable> {
            class1.groupSelector.select(3)
        }.message)
        Assertions.assertTrue(class1.students.containsAll(class1.groupSelector.select(1)))
    }
}

