package com.github.immersingeducation.immersingpicker.backend

import com.github.immersingeducation.immersingpicker.backend.selectors.GroupSelector
import com.github.immersingeducation.immersingpicker.backend.selectors.StudentSelector
import mu.KotlinLogging
import java.util.PriorityQueue
import java.util.Random

data class ClassNGrade(
    val name: String,
    val students: MutableList<Student>
) {
    companion object {
        val logger = KotlinLogging.logger {}
    }

    val groupStudentMap = mutableMapOf<String, MutableList<Student>>()
//    val selectByStudentAndGroupPQ = PriorityQueue<Student>(compareBy { it.weight })

    val studentSelector = StudentSelector(this)
    val groupSelector = GroupSelector(this)

    init {
        findGroups()
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

    fun smallestUnusedId(): Int {
        val s = students.sortedBy { it.id }
        var id = 1
        for (it in s) {
            if (it.id != id) {
                break
            } else {
                id ++
            }
        }
        logger.trace("查找到目前最小的学号为 $id")
        return id
    }

    fun addStudent(name: String, id: Int, gender: String, group: String, seat: Pair<Int, Int>) {
        if (!checkIfIdExists(id)) {
            val s = Student(name, id, gender, group, seat)
            students.add(s)
            if (groupStudentMap[group] == null) {
                groupStudentMap[group] = mutableListOf(s)
            } else {
                groupStudentMap[group]!!.add(s)
            }
            logger.trace("成功添加学生：{}", s)
        } else {
            throw IllegalArgumentException("已存在学号为 $id 的学生，无法再次新建。")
        }
    }

    fun findGroups() {
        students.forEach {
            if (groupStudentMap[it.group] == null) {
                logger.trace("查询到新的小组及其成员：${it.group}")
                groupStudentMap[it.group] = mutableListOf(it)
            } else {
                logger.trace("查询到小组 ${it.group} 的新成员")
                groupStudentMap[it.group]!!.add(it)
            }
        }
        logger.debug("成功建立小组映射")
    }



    constructor(name: String): this(
        name = name,
        students = mutableListOf()
    )
}