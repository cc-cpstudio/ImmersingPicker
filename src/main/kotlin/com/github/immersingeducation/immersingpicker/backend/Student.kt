package com.github.immersingeducation.immersingpicker.backend

data class Student(
    var name: String,
    val id: Int,
    var gender: String,
    var group: String,
    var seat: Pair<Int, Int>
) {
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
        if (gender.length > 10) {
            throw IllegalArgumentException("性别长度不能大于10")
        }
        if (group.length > 50) {
            throw IllegalArgumentException("小组长度不能大于50")
        }
        if (seat.first <= 0 || seat.second <= 0) {
            throw IllegalArgumentException("座位行列数必须为正整数")
        }
    }
}