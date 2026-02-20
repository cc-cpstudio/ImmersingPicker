package com.github.immersingeducation.immersingpicker.data

import com.github.immersingeducation.immersingpicker.core.Student
import java.time.LocalDateTime

data class StorableStudent(
    val name: String,
    val id: Int,
    val seatRow: Int,
    val seatColumn: Int,
    val initialWeight: Double,
    val lastSelectedTime: LocalDateTime,
    val selectedAmount: Int,
    val weight: Double
) {
    companion object {
        fun deserialize(storable: StorableStudent): Student {
            return Student(
                name = storable.name,
                id = storable.id,
                seatRow = storable.seatRow,
                seatColumn = storable.seatColumn,
            ).apply {
                initialWeight = storable.initialWeight
                lastSelectedTime = storable.lastSelectedTime
                selectedAmount = storable.selectedAmount
                weight = storable.weight
            }
        }
    }
}