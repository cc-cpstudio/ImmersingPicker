package com.github.immersingeducation.immersingpicker.data.clazz

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.Student

object TransitionUtils {
    fun listToClasses(classes: List<Clazz>, currentIndex: Int?): Classes {
        return Classes(classes.map{ clazzToStorable(it) }.toList(), currentIndex)
    }

    fun classesToList(classes: Classes): List<Clazz> {
        return classes.classes.map{ storableToClazz(it) }.toList()
    }

    fun classesToCurrentIndex(classes: Classes): Int? {
        return classes.currentIndex
    }

    fun clazzToStorable(clazz: Clazz): StorableClazz {
        return StorableClazz(
            name = clazz.name,
            students = clazz.students.map { studentToStorable(it) }.toMutableList(),
            historyList = clazz.historyList
        )
    }

    fun storableToClazz(storable: StorableClazz): Clazz {
        return Clazz(
            name = storable.name,
            students = storable.students.map { storableToStudent(it) }.toMutableList(),
            historyList = storable.historyList.toMutableList()
        )
    }

    fun studentToStorable(student: Student): StorableStudent {
        return StorableStudent(
            name = student.name,
            id = student.id,
            seatRow = student.seatRow,
            seatColumn = student.seatColumn,
            initialWeight = student.initialWeight,
            lastSelectedTime = student.lastSelectedTime,
            selectedAmount = student.selectedAmount,
            weight = student.weight
        )
    }

    fun storableToStudent(storable: StorableStudent): Student {
        return Student(
            name = storable.name,
            id = storable.id,
            seatRow = storable.seatRow,
            seatColumn = storable.seatColumn
        ).apply {
            initialWeight = storable.initialWeight
            lastSelectedTime = storable.lastSelectedTime
            selectedAmount = storable.selectedAmount
            weight = storable.weight
        }
    }
}