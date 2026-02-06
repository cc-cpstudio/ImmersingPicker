package com.github.immersingeducation.immersingpicker.backend.selectors

import com.github.immersingeducation.immersingpicker.backend.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.Student
import java.util.Random

abstract class SelectorBase(val clazz: ClassNGrade) {
    abstract val name: String

    val students: MutableList<Student>
        get() = clazz.students
    val groupStudentMap: Map<String, List<Student>>
        get() = clazz.groupStudentMap

    val random = Random()

    abstract fun select(amount: Int): List<Student>
}