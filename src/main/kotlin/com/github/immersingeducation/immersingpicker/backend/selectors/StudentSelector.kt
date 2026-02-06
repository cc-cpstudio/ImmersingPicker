package com.github.immersingeducation.immersingpicker.backend.selectors

import com.github.immersingeducation.immersingpicker.backend.core.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.core.ClassNGrade.Companion.logger
import com.github.immersingeducation.immersingpicker.backend.core.History
import com.github.immersingeducation.immersingpicker.backend.core.Student
import java.util.PriorityQueue

class StudentSelector(clazz: ClassNGrade): SelectorBase(clazz) {
    override val name: String = "StudentSelector"

    val selectByStudentAndGroupPQ = PriorityQueue<Student>(compareBy { it.weight })

    init {
        for (student in students) {
            student.weight = random.nextDouble()
            selectByStudentAndGroupPQ.offer(student)
        }
    }

    override fun select(amount: Int): History {
        if (amount >= students.size) {
            logger.warn("所需的抽取数量 $amount 不允许大于班级人数 ${students.size}，暂且给你报个错")
            throw IllegalArgumentException("所需的抽取数量 $amount 不允许大于班级人数 ${students.size}")
        } else {
            val res = mutableListOf<Student>()


            for (i in 1..amount) {
                var selected = selectByStudentAndGroupPQ.poll()
                while (res.contains(selected)) {
                    selected.weight = random.nextDouble()
                    selectByStudentAndGroupPQ.offer(selected)
                    selected = selectByStudentAndGroupPQ.poll()
                }

                logger.trace("当前已抽选：$i 人；已选中：id=${selected.id}, weight=${selected.weight}")
                res.add(selected)
                selected.weight = random.nextDouble()
                selectByStudentAndGroupPQ.offer(selected)
            }
            logger.trace("抽选完成，即将创建历史记录")
            val history = History(
                selector = name,
                students = res,
            )
            historyList.add(history)
            logger.info("抽选完成，即将传送抽选数据")
            return history
        }
    }

}