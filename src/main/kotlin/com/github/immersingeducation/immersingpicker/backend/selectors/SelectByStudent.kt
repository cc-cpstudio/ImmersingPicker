package com.github.immersingeducation.immersingpicker.backend.selectors

import com.github.immersingeducation.immersingpicker.backend.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.ClassNGrade.Companion.logger
import com.github.immersingeducation.immersingpicker.backend.Student
import java.util.Random

//val ClassNGrade.selectByStudentPQ: PriorityQueue<Student>
//    get() = PriorityQueue<Student>(compareByDescending { it.weight })

fun ClassNGrade.selectByStudent(amount: Int): List<Student> {
    if (amount >= students.size) {
        logger.warn("所需的抽取数量 $amount 不允许大于班级人数 ${students.size}，暂且给你报个错")
        throw IllegalArgumentException("所需的抽取数量 $amount 不允许大于班级人数 ${students.size}")
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

            logger.trace("当前已抽选：$i 人；已选中：id=${selected.id}, weight=${selected.weight}")
            res.add(selected)
            selected.weight = random.nextDouble()
            selectByStudentAndGroupPQ.offer(selected)
        }
        logger.info("抽选完成，即将传送抽选数据")
        return res
    }
}

