package com.github.immersingeducation.immersingpicker.selectors

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.History
import com.github.immersingeducation.immersingpicker.core.Student
import java.util.PriorityQueue

/**
 * 学生选择器，根据班级中的学生和历史记录选择指定数量的学生
 * @param clazz 班级对象，用于操作班级中的学生和历史记录
 * @author CC想当百大
 * @since v1.0.0.a
 */
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
