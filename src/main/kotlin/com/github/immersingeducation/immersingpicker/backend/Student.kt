package com.github.immersingeducation.immersingpicker.backend

data class Student(
    var name: String,
    val id: Int,
    var gender: String,
    var group: String,
    val seat: Pair<Int, Int>
) {
    var weight: Double = -1.0

    init {
        if (id <= 0) {
            throw IllegalArgumentException("学号必须为正整数")
        }
        if (seat.first <= 0 || seat.second <= 0) {
            throw IllegalArgumentException("座位行列数必须为正整数")
        }
    }
}