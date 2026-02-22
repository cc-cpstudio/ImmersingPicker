package com.github.immersingeducation.immersingpicker.data.clazz

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.Student

/**
 * 班级转换工具类，用于将班级列表和当前操作的班级索引转换为可存储的班级类和班级列表
 * @author CC想当百大
 * @since v1.0.0.a
 */
object TransitionUtils {
    /**
     * 将班级列表和当前操作的班级索引转换为可存储的班级类和班级列表
     * @param classes 班级列表
     * @param currentIndex 当前操作的班级索引
     * @return 可存储的班级类和班级列表
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun listToClasses(classes: List<Clazz>, currentIndex: Int?): Classes {
        return Classes(classes.map{ clazzToStorable(it) }.toList(), currentIndex)
    }

    /**
     * 将可存储的班级类和班级列表转换为班级列表和当前操作的班级索引
     * @param classes 可存储的班级类和班级列表
     * @return 班级列表和当前操作的班级索引
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun classesToList(classes: Classes): List<Clazz> {
        return classes.classes.map{ storableToClazz(it) }.toList()
    }

    /**
     * 将可存储的班级类和班级列表转换为当前操作的班级索引
     * @param classes 可存储的班级类和班级列表
     * @return 当前操作的班级索引
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun classesToCurrentIndex(classes: Classes): Int? {
        return classes.currentIndex
    }

    /**
     * 将班级类转换为可存储的班级类
     * @param clazz 班级类
     * @return 可存储的班级类
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun clazzToStorable(clazz: Clazz): StorableClazz {
        return StorableClazz(
            name = clazz.name,
            students = clazz.students.map { studentToStorable(it) }.toMutableList(),
            historyList = clazz.historyList
        )
    }

    /**
     * 将可存储的班级类转换为班级类
     * @param storable 可存储的班级类
     * @return 班级类
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun storableToClazz(storable: StorableClazz): Clazz {
        return Clazz(
            name = storable.name,
            students = storable.students.map { storableToStudent(it) }.toMutableList(),
            historyList = storable.historyList.toMutableList()
        )
    }

    /**
     * 将学生类转换为可存储的学生类
     * @param student 学生类
     * @return 可存储的学生类
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun studentToStorable(student: Student): StorableStudent {
        return StorableStudent(
            name = student.name,
            id = student.id,
            seatRow = student.seatRow,
            seatColumn = student.seatColumn,
            initialWeight = student.initialWeight,
            lastSelectedTime = student.lastSelectedTime,
            selectedAmount = student.selectedAmount,
            weight = student.weight
        )
    }

    /**
     * 将可存储的学生类转换为学生类
     * @param storable 可存储的学生类
     * @return 学生类
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun storableToStudent(storable: StorableStudent): Student {
        return Student(
            name = storable.name,
            id = storable.id,
            seatRow = storable.seatRow,
            seatColumn = storable.seatColumn
        ).apply {
            initialWeight = storable.initialWeight
            lastSelectedTime = storable.lastSelectedTime
            selectedAmount = storable.selectedAmount
            weight = storable.weight
        }
    }
}