package com.github.immersingeducation.immersingpicker.backend.selectors

import com.github.immersingeducation.immersingpicker.backend.core.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.core.History
import com.github.immersingeducation.immersingpicker.backend.core.Student
import java.util.Random

abstract class SelectorBase(val clazz: ClassNGrade) {
    abstract val name: String

    val students: MutableList<Student>
        get() = clazz.students
    val groupStudentMap: Map<String, List<Student>>
        get() = clazz.groupStudentMap
    val historyList: MutableList<History>
        get() = clazz.historyList

    val random = Random()

    abstract fun select(amount: Int): History
}