package com.github.immersingeducation.immersingpicker.core

import com.github.immersingeducation.immersingpicker.selectors.StudentSelector
import mu.KotlinLogging
import java.util.Random

data class Clazz(
    val name: String,
    val students: MutableList<Student>,
    val historyList: MutableList<History>
) {
    companion object {
        val logger = KotlinLogging.logger {}
        val classes = mutableListOf<Clazz>()
        var currentIndex: Int? = null

        fun getCurrentClass(): Clazz? {
            return currentIndex?.let { classes[it] }
        }
    }

    val random = Random()
    val studentSelector = StudentSelector(this)

    init {
        classes.add(this)
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
            val s = Student(name, id, seat.first, seat.second)
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

    constructor(name: String): this(
        name = name,
        students = mutableListOf(),
        historyList = mutableListOf()
    )
}