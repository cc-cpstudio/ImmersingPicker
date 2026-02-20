package data

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.Student
import com.github.immersingeducation.immersingpicker.data.clazz.ClazzStorageUtils
import com.github.immersingeducation.immersingpicker.data.clazz.TransitionUtils

fun main() {
    val clazz = Clazz(
        name = "class1",
        students = mutableListOf(
            Student(
                name = "student1",
                id = 1,
                seatRow = 1,
                seatColumn = 1
            ),
            Student(
                name = "student2",
                id = 2,
                seatRow = 2,
                seatColumn = 2
            )
        ),
        historyList = mutableListOf()
    )
    ClazzStorageUtils.saveClasses()
}