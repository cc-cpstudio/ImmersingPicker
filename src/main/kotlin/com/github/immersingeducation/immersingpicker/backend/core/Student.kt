package com.github.immersingeducation.immersingpicker.backend.core

import java.time.LocalDateTime

data class Student(
    var name: String,
    val id: Int,
    var initialWeight: Double,
    var seatRow: Int,
    var seatColumn: Int
) {
    var lastSelectedTime: LocalDateTime? = null

    var selectedAmount: Int = 0


    var weight: Double = -1.0
        set(value) {
            if (value > 0.0) {
                field = value
            } else {
                throw IllegalArgumentException("权重必须为正数")
            }
        }

    init {
        if (name.length > 255) {
            throw IllegalArgumentException("姓名长度不能大于255")
        }
        if (id <= 0) {
            throw IllegalArgumentException("学号必须为正整数")
        }
    }
}