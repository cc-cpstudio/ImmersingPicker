package com.github.immersingeducation.immersingpicker.component

import com.github.immersingeducation.immersingpicker.core.ClassNGrade
import com.github.immersingeducation.immersingpicker.core.Student
import javafx.geometry.Insets
import javafx.scene.layout.GridPane

class SeatGrid(val clazz: ClassNGrade): GridPane() {
    val seats = mutableListOf<Seat>()

    init {
        hgap = 5.0; vgap = 5.0
        padding = Insets(5.0)

        refresh()
    }

    private fun addStudent(student: Student) {
        if (student.seatRow >= 0 && student.seatColumn > 0) {
            val s = Seat(student)
            seats.add(s)
            add(s.root, student.seatColumn - 1, student.seatRow)
        }
    }

    fun refresh() {
        children.clear()
        seats.clear()
        clazz.students.forEach {
            addStudent(it)
        }
    }
}