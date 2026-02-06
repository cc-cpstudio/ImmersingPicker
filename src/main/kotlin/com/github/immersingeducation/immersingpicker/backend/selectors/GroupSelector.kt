package com.github.immersingeducation.immersingpicker.backend.selectors

import com.github.immersingeducation.immersingpicker.backend.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.ClassNGrade.Companion.logger
import com.github.immersingeducation.immersingpicker.backend.Student
import java.util.PriorityQueue
import java.util.Random

class GroupSelector(clazz: ClassNGrade): SelectorBase(clazz) {
    override val name: String = "GroupSelector"

    val selectByStudentAndGroupPQ = PriorityQueue<Student>(compareBy { it.weight })

    init {
        for (student in students) {
            student.weight = random.nextDouble()
            selectByStudentAndGroupPQ.offer(student)
        }
    }

    override fun select(amount: Int): List<Student> {
        if (amount > groupStudentMap.size) {
            logger.warn("所需的抽取数量 $amount 不允许大于班级组数 ${groupStudentMap.size}，暂且给你报个错")
            throw IllegalArgumentException("所需的抽取数量 $amount 不允许大于班级组数 ${groupStudentMap.size}")
        } else {
            val res = mutableListOf<Student>()
            val random = Random()
            for (i in 1..amount) {
                var selected = selectByStudentAndGroupPQ.poll()
                while (res.contains(selected)) {
                    selected.weight = random.nextDouble()
                    selectByStudentAndGroupPQ.offer(selected)
                    selected = selectByStudentAndGroupPQ.poll()
                }

                logger.trace("当前已抽选：$i 组；已选中：group=${selected.group}, weight=${selected.weight}")
                students.forEach {
                    if (it.group == selected.group) {
                        res.add(it)
                    }
                    selected.weight = random.nextDouble()
                    selectByStudentAndGroupPQ.offer(selected)
                }
            }
            logger.info("抽选完成，即将传送抽选数据")
            return res
        }
    }
}