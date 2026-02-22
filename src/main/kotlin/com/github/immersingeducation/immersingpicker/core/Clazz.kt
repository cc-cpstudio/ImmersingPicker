package com.github.immersingeducation.immersingpicker.core

import com.github.immersingeducation.immersingpicker.selectors.StudentSelector
import mu.KotlinLogging
import java.util.Random

/**
 * 班级类，用于表示班级信息和操作
 * @param name 班级名称
 * @param students 班级学生列表
 * @param historyList 班级历史记录列表
 * @author CC想当百大
 * @since v1.0.0.a
 */
data class Clazz(
    val name: String,
    val students: MutableList<Student>,
    val historyList: MutableList<History>
) {
    companion object {
        val logger = KotlinLogging.logger {}

        /**
         * 班级列表，用于存储所有班级对象
         * @author CC想当百大
         * @since v1.0.0.a
         */
        val classes = mutableListOf<Clazz>()

        /**
         * 当前班级索引，用于指向当前操作的班级对象，没有则为 null
         * @author CC想当百大
         * @since v1.0.0.a
         */
        var currentIndex: Int? = null

        /**
         * 获取当前班级对象
         * @return 当前班级对象，如果没有当前班级则返回 null
         * @author CC想当百大
         * @since v1.0.0.a
         */
        fun getCurrentClazz(): Clazz? {
            return currentIndex?.let { classes[it] }
        }

        /**
         * 获取指定索引的班级对象
         * @param index 班级索引
         * @return 指定索引的班级对象，如果索引无效则返回 null
         * @author CC想当百大
         * @since v1.0.0.a
         */
        fun getClazz(index: Int): Clazz? {
            return if (index in classes.indices) {
                classes[index]
            } else {
                null
            }
        }
    }

    val random = Random()
    val studentSelector = StudentSelector(this)

    init {
        classes.add(this)
        logger.info("成功创建班级：$name")
    }

    /**
     * 检查班级是否存在指定学号的学生
     * @param id 要检查的学号
     * @return 如果班级存在指定学号的学生则返回 true，否则返回 false
     * @author CC想当百大
     * @since v1.0.0.a
     */
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

    /**
     * 添加学生到班级
     * @param name 学生姓名
     * @param id 学生学号
     * @param seat 学生座位坐标
     * @throws IllegalArgumentException 如果班级已存在指定学号的学生
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun addStudent(name: String, id: Int, seat: Pair<Int, Int>) {
        if (!checkIfIdExists(id)) {
            val s = Student(name, id, seat.first, seat.second)
            students.add(s)
            logger.trace("成功添加学生：id=${s.id}")
        } else {
            throw IllegalArgumentException("已存在学号为 $id 的学生，无法再次新建。")
        }
    }

    /**
     * 从班级中删除指定学号的学生
     * @param id 要删除的学生学号
     * @throws IllegalArgumentException 如果班级不存在指定学号的学生
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun removeStudent(id: Int) {
        if (checkIfIdExists(id)) {
            students.remove(students.find { it.id == id})
        } else {
            throw IllegalArgumentException("不存在学号为 $id 的学生，无法删除。")
        }
    }

    /**
     * 创建一个空班级，班级名称为 [name]
     * @param name 班级名称
     * @author CC想当百大
     * @since v1.0.0.a
     */
    constructor(name: String): this(
        name = name,
        students = mutableListOf(),
        historyList = mutableListOf()
    )
}