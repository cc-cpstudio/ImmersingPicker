package com.github.immersingeducation.immersingpicker.component

import com.github.immersingeducation.immersingpicker.core.ClassNGrade
import com.github.immersingeducation.immersingpicker.core.Student
import javafx.geometry.Insets
import javafx.scene.layout.GridPane

class SeatGrid(val clazz: ClassNGrade): GridPane() {
    init {
        hgap = 5.0; vgap = 5.0
        padding = Insets(5.0)

        update()
    }

    private fun addStudent(student: Student) {
        if (student.seatRow >= 0 && student.seatColumn > 0) {
            add(Seat(student).root, student.seatColumn - 1, student.seatRow)
        }
    }

    fun update() {
        children.clear()
        clazz.students.forEach {
            addStudent(it)
        }
    }
}