package com.github.immersingeducation.immersingpicker.backend.core

import com.github.immersingeducation.immersingpicker.backend.selectors.StudentSelector
import mu.KotlinLogging
import java.time.LocalDateTime
import java.time.temporal.ChronoUnit
import java.util.Random
import kotlin.math.pow

data class ClassNGrade(
    val name: String,
    val students: MutableList<Student>,
    val historyList: MutableList<History>
) {
    companion object {
        val logger = KotlinLogging.logger {}
    }

    val random = Random()

    val studentSelector = StudentSelector(this)

    init {
        logger.info("成功创建班级：$name")
    }

    fun checkIfIdExists(id: Int): Boolean {
        var flag = false
        for (it in students) {
            if (it.id == id) {
                flag = true
                break
            }
        }
        logger.trace(if (flag) {
            "$name 已查找到学号为 $id 的学生存在。"
        } else {
            "$name 未找到学号为 $id 的学生，判定为不存在"
        })
        return flag
    }

    fun addStudent(name: String, id: Int, seat: Pair<Int, Int>) {
        if (!checkIfIdExists(id)) {
            val s = Student(name, id, 1.0, seat.first, seat.second)
            students.add(s)
            logger.trace("成功添加学生：id=${s.id}")
        } else {
            throw IllegalArgumentException("已存在学号为 $id 的学生，无法再次新建。")
        }
    }

    fun removeStudent(id: Int) {
        if (checkIfIdExists(id)) {
            students.remove(students.find { it.id == id})
        } else {
            throw IllegalArgumentException("不存在学号为 $id 的学生，无法删除。")
        }
    }

    private fun calculateRange(needed: List<Student>): Int {
        val maxE = students.maxBy { it.selectedAmount }
        val minE = students.minBy { it.selectedAmount }
        return maxE.selectedAmount - minE.selectedAmount
    }

    private fun calculateAvailableSelectStudents(): MutableList<Student> {
        val tmpStudents = students
        var availableRange = calculateRange(tmpStudents)
        while (availableRange > MAX_AVAIL_RANGE && tmpStudents.size >= MIN_SELECTION_POOL_AMOUNT) {
            tmpStudents.remove(students.maxBy { it.selectedAmount })
            availableRange = calculateRange(tmpStudents)
        }
        val average = students.sumOf { it.selectedAmount } / students.size
        tmpStudents.forEach {
            if (it.selectedAmount >= average) {
                students.remove(it)
            }
        }
        return tmpStudents
    }

    fun calculateWeight() {
        // 权重影响因素：抽取次数，上次抽取时间与现在的间隔，
        val available = calculateAvailableSelectStudents()
        val average = available.sumOf { it.selectedAmount } / available.size
        students.forEach {
            if (it.weight <= 1.0) {
                it.weight  = 1.0
            }
        }
        students.forEach {
            if (available.contains(it)) {
                it.weight += (average - it.selectedAmount) * 1.7
            } else {
                it.weight += 0.8
            }
            if (it.lastSelectedTime != null && ChronoUnit.DAYS.between(LocalDateTime.now(), it.lastSelectedTime) > 3) {
                it.weight += 1.12.pow(ChronoUnit.DAYS.between(it.lastSelectedTime, LocalDateTime.now()).toDouble())
            } else {
                it.weight += ChronoUnit.DAYS.between(LocalDateTime.now(), LocalDateTime.now()).toDouble() * 1.01
            }
            it.weight += random.nextDouble(1.0, 5.0)
        }
    }

    constructor(name: String): this(
        name = name,
        students = mutableListOf(),
        historyList = mutableListOf()
    )
}