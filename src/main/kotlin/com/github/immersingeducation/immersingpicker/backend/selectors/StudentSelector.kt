package com.github.immersingeducation.immersingpicker.backend.selectors

import com.github.immersingeducation.immersingpicker.backend.core.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.core.History
import com.github.immersingeducation.immersingpicker.backend.core.Student
import java.util.PriorityQueue

class StudentSelector(clazz: ClassNGrade): SelectorBase(clazz) {
    override val name: String = "StudentSelector"

    override fun selectLogic(amount: Int): History {
        val pq = PriorityQueue<Student>(compareByDescending { it.weight })
        val selected = mutableListOf<Student>()
        clazz.students.forEach {
            pq.add(it)
        }
        for (i in 1..amount) {
            selected.add(pq.poll())
        }
        return History(
            selector = name,
            students = selected
        )
    }
}

fun main() {
    val clazz = ClassNGrade("name", mutableListOf<Student>(Student("name", 1, 2.0, 1, 1), Student("nam", 2, 3.0, 1, 2)), mutableListOf<History>())
    println(StudentSelector(clazz).select(1))
}