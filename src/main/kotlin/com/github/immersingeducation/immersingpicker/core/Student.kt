package com.github.immersingeducation.immersingpicker.core

import mu.KotlinLogging
import java.time.LocalDateTime

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
        if (name.length > 255) {
            throw IllegalArgumentException("姓名长度不能大于255")
        }
        if (id <= 0) {
            throw IllegalArgumentException("学号必须为正整数")
        }
        logger.trace("成功创建学生：id=$id, name=$name, seat=($seatRow, $seatColumn)")
    }
}