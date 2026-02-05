package com.github.immersingeducation.immersingpicker.backend.selectors

import com.github.immersingeducation.immersingpicker.backend.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.ClassNGrade.Companion.logger
import com.github.immersingeducation.immersingpicker.backend.NO_GROUP
import com.github.immersingeducation.immersingpicker.backend.Student
import java.util.Random

//fun ClassNGrade.selectByGroup(amount: Int): Map<String, List<Student>> {
//    val res = mutableMapOf<String, List<Student>>()
//    if (amount >= groupStudentMap.size) {
//        ClassNGrade.logger.warn("所需的抽取数量 $amount 大于等于班级组数 ${groupStudentMap.size}，暂且送你一个小组名单。")
//        return groupStudentMap
//    }
//    for (i in 1..amount) {
//        val group = groupStudentMap.keys.random()
//        if (group == NO_GROUP) {
//            ClassNGrade.logger.trace("未分组成员，跳过")
//            continue
//        } else {
//            ClassNGrade.logger.trace("当前已抽选 $i 组；已选中：group=$group")
//            res[group] = groupStudentMap[group] as List<Student>
//        }
//    }
//    ClassNGrade.logger.info("抽选完成，即将传送抽选数据")
//    return res
//}

fun ClassNGrade.selectByGroup(amount: Int): List<Student> {
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