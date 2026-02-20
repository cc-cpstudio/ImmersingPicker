package com.github.immersingeducation.immersingpicker.data.clazz

import com.github.immersingeducation.immersingpicker.core.Student
import java.time.LocalDateTime

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