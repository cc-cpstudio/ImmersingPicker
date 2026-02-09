package com.github.immersingeducation.immersingpicker.backend.selectors

import com.github.immersingeducation.immersingpicker.backend.core.ClassNGrade
import com.github.immersingeducation.immersingpicker.backend.core.History
import com.github.immersingeducation.immersingpicker.backend.core.Student
import java.util.Random

abstract class SelectorBase(val clazz: ClassNGrade) {
    companion object{
        val random = Random()
    }

    abstract val name: String

    abstract fun selectLogic(amount: Int): History

    fun select(amount: Int): History {
        clazz.calculateWeight()
        val history = selectLogic(amount)
        history.students.forEach {
            it.lastSelectedTime = history.createTime
            it.selectedAmount ++
        }
        return history
    }
}