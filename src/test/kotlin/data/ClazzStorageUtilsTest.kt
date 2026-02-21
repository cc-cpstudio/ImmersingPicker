package data

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.Student
import com.github.immersingeducation.immersingpicker.data.clazz.ClazzStorageUtils
import java.time.LocalDateTime

fun main() {
    val clazz = Clazz(
        name = "class1",
        students = mutableListOf(
            Student(
                name = "student1",
                id = 1,
                seatRow = 1,
                seatColumn = 1
            ).apply {
                lastSelectedTime = LocalDateTime.now()
            },
            Student(
                name = "student2",
                id = 2,
                seatRow = 2,
                seatColumn = 2
            ).apply {
                lastSelectedTime = LocalDateTime.now()
            }
        ),
        historyList = mutableListOf()
    )
    ClazzStorageUtils.saveClasses()
    ClazzStorageUtils.loadClasses()
}