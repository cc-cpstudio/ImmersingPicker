package backend

import com.github.immersingeducation.immersingpicker.backend.core.NO_GENDER
import com.github.immersingeducation.immersingpicker.backend.core.NO_GROUP
import com.github.immersingeducation.immersingpicker.backend.core.Student
import org.junit.jupiter.api.Assertions.assertEquals
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.assertThrows

class StudentTest {
    @Test
    fun `test init`() {
        val s1 = Student("John", 1, NO_GENDER, NO_GROUP, Pair(1, 1))
        assertEquals("John", s1.name)
        assertEquals(1, s1.id)
        assertEquals(NO_GENDER, s1.gender)
        assertEquals(NO_GROUP, s1.group)
        assertEquals(Pair(1, 1), s1.seat)

        assertEquals("学号必须为正整数", assertThrows<Throwable> { Student("a", -1, NO_GENDER, NO_GROUP, Pair(1, 2)) }.message)
        assertEquals("座位行列数必须为正整数", assertThrows<Throwable> { Student("John", 1, NO_GENDER, NO_GROUP, Pair(-1, 1)) }.message)
    }

    @Test
    fun `test weight setter`() {
        val s1 = Student("John", 1, NO_GENDER, NO_GROUP, Pair(1, 1))
        assertEquals(-1.0, s1.weight)
        s1.weight = 2.0
        assertEquals(2.0, s1.weight)
        assertEquals("权重必须为正数", assertThrows<Throwable> {
            s1.weight = -1.3
        }.message)
    }
}