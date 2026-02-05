package backend

import com.github.immersingeducation.immersingpicker.backend.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.NO_GENDER
import com.github.immersingeducation.immersingpicker.backend.NO_GROUP
import com.github.immersingeducation.immersingpicker.backend.Student
import org.junit.jupiter.api.Assertions.assertEquals
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.assertThrows

class ClassNGradeTeat {
    fun constructStudent(): MutableList<Student> {
        val s1 = Student(name = "s1", id = 1, gender = NO_GENDER, group = "g1", seat = Pair(1, 1))
        val s2 = Student(name = "s2", id = 2, gender = NO_GENDER, group = "g1", seat = Pair(1, 2))
        val s3 = Student(name = "s4", id = 4, gender = NO_GENDER, group = "g2", seat = Pair(1, 3))
        return mutableListOf(s1, s2, s3)
    }

    @Test
    fun `test init`() {
        val class1 = ClassNGrade(name = "class1")
        assertEquals("class1", class1.name)
        assertEquals(0, class1.students.size)

        val class2 = ClassNGrade(name = "class2", students = constructStudent())
        assertEquals("class2", class2.name)
        assertEquals(3, class2.students.size)
    }

    @Test
    fun `test check if id exists`() {
        val class1 = ClassNGrade(name = "class1", students = constructStudent())
        assertEquals(true, class1.checkIfIdExists(1))
        assertEquals(false, class1.checkIfIdExists(3))
    }

    @Test
    fun `test smallest unused id`() {
        val class1 = ClassNGrade(name = "class1")
        assertEquals(1, class1.smallestUnusedId())

        val class2 = ClassNGrade(name = "class2", students = constructStudent())
        assertEquals(3, class2.smallestUnusedId())
    }

    @Test
    fun `test add student`() {
        val class1 = ClassNGrade(name = "class1", students = constructStudent())
        class1.addStudent(name = "st3", id = 3, gender = NO_GENDER, group = NO_GROUP, seat = Pair(1, 4))
        assertEquals(Student(name = "st3", id = 3, gender = NO_GENDER, group = NO_GROUP, seat = Pair(1, 4)), class1.students[3])
        assertEquals("已存在学号为 4 的学生，无法再次新建。", assertThrows<Throwable> { class1.addStudent(name = "s4", id = 4, gender = NO_GENDER, group = NO_GROUP, seat = Pair(1, 3)) }.message)
    }

}