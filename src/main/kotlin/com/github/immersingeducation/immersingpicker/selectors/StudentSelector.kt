package com.github.immersingeducation.immersingpicker.selectors

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.History
import com.github.immersingeducation.immersingpicker.core.Student
import java.util.PriorityQueue

class StudentSelector(clazz: Clazz): SelectorBase(clazz) {
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
