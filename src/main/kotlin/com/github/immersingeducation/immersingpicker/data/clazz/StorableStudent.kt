package com.github.immersingeducation.immersingpicker.data.clazz

import com.github.immersingeducation.immersingpicker.core.Student
import java.time.LocalDateTime

/**
 * 可存储的学生类，用于表示班级中的学生列表
 * @param name 学生姓名
 * @param id 学生学号
 * @param seatRow 学生座位行号
 * @param seatColumn 学生座位列号
 * @param initialWeight 学生初始权重
 * @param lastSelectedTime 学生上次抽选时间
 * @param selectedAmount 学生抽选次数
 * @param weight 学生当前权重
 * @author CC想当百大
 * @since v1.0.0.a
 */
data class StorableStudent(
    var name: String,
    var id: Int,
    var seatRow: Int,
    var seatColumn: Int,
    var initialWeight: Double,
    var lastSelectedTime: LocalDateTime?,
    var selectedAmount: Int,
    var weight: Double
)