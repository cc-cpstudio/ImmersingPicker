package com.github.immersingeducation.immersingpicker.data.clazz

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.History

data class StorableClazz(
    var name: String,
    var students: MutableList<StorableStudent>,
    var historyList: List<History>
)