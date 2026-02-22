package com.github.immersingeducation.immersingpicker.core

import mu.KotlinLogging
import java.time.LocalDateTime

/**
 * 学生类，用于表示班级中的学生信息
 * @param name 学生姓名
 * @param id 学生学号
 * @param seatRow 学生座位行号
 * @param seatColumn 学生座位列号
 * @throws IllegalArgumentException 如果学号小于等于0
 * @author CC想当百大
 * @since v1.0.0.a
 */
data class Student(
    var name: String,
    val id: Int,
    var seatRow: Int,
    var seatColumn: Int
) {
    companion object {
        val logger = KotlinLogging.logger {}
    }

    var initialWeight = 1.0
    var lastSelectedTime: LocalDateTime? = null
    var selectedAmount: Int = 0
    var weight: Double = -1.0

    init {
        if (id <= 0) {
            throw IllegalArgumentException("学号必须为正整数")
        }
        logger.trace("成功创建学生：id=$id, name=$name, seat=($seatRow, $seatColumn)")
    }
}