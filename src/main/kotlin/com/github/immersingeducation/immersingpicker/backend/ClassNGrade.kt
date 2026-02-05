package com.github.immersingeducation.immersingpicker.backend

import mu.KotlinLogging
import java.util.PriorityQueue

data class ClassNGrade(
    val name: String,
    val students: MutableList<Student>
) {
    companion object {
        val logger = KotlinLogging.logger {}
    }

    init {
        logger.debug("成功创建班级：$name")
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
            logger.trace("成功添加学生：{}", s)
        } else {
            throw IllegalArgumentException("已存在学号为 $id 的学生，无法再次新建。")
        }
    }

    fun findGroups(): Map<String, List<Student>> {
        val groupStudentMap = mutableMapOf<String, MutableList<Student>>()
        students.forEach {
            if (groupStudentMap[it.group] == null) {
                logger.trace("查询到新的小组及其成员：${it.group}")
                groupStudentMap[it.group] = mutableListOf(it)
            } else {
                logger.trace("查询到小组 ${it.group} 的新成员")
                groupStudentMap[it.group]!!.add(it)
            }
        }
        return groupStudentMap
    }

    fun selectByStudent(amount: Int): List<Student> {
        if (amount >= students.size) {
            logger.warn("所需的抽取数量 $amount 大于等于班级人数 ${students.size}，暂且送你一个学生名单")
            return students
        } else {
            val res = mutableListOf<Student>()
            for (i in 1..amount) {
                val selected = students.random()
                logger.trace("当前已抽选：$i 人；已选中：id=${selected.id}")
                res.add(selected)
            }
            logger.debug("抽选完成，即将传送抽选数据")
            return res
        }
    }

    fun selectByGroup(amount: Int): Map<String, List<Student>> {
        val res = mutableMapOf<String, List<Student>>()
        val groupStudentMap = findGroups()
        if (amount >= groupStudentMap.size) {
            logger.warn("所需的抽取数量 $amount 大于等于班级组数 ${groupStudentMap.size}，暂且送你一个小组名单。")
            return groupStudentMap
        }
        for (i in 1..amount) {
            val group = groupStudentMap.keys.random()
            if (group == NO_GROUP) {
                logger.trace("未分组成员，跳过")
                continue
            } else {
                logger.trace("当前已抽选 $i 组；已选中：group=$group")
                res[group] = groupStudentMap[group] as List<Student>
            }
        }
        logger.debug("抽选完成，即将传送抽选数据")
        return res
    }

    constructor(name: String): this(
        name = name,
        students = mutableListOf()
    )
}