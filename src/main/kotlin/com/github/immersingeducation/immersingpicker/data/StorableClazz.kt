package com.github.immersingeducation.immersingpicker.data

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.History

data class StorableClazz(
    val name: String,
    val students: List<StorableStudent>,
    val historyList: List<History>
) {
    companion object {
        fun deserialize(storable: StorableClazz): Clazz {
            return Clazz(
                name = storable.name,
                students = storable.students.map { StorableStudent.deserialize(it) }.toMutableList(),
                historyList = storable.historyList.toMutableList()
            )
        }
        
        fun serialize(clazz: Clazz): StorableClazz {
            return StorableClazz(
                name = clazz.name,
                students = clazz.students.map { StorableStudent.serialize(it) },
                historyList = clazz.historyList
            )
        }
    }
}