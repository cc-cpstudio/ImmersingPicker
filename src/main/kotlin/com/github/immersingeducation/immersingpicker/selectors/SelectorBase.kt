package com.github.immersingeducation.immersingpicker.selectors

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.History
import com.github.immersingeducation.immersingpicker.core.MAX_AVAIL_RANGE
import com.github.immersingeducation.immersingpicker.core.MIN_SELECTION_POOL_AMOUNT
import com.github.immersingeducation.immersingpicker.core.Student
import java.time.LocalDateTime
import java.time.temporal.ChronoUnit
import java.util.Random
import kotlin.math.pow

abstract class SelectorBase(val clazz: Clazz) {
    companion object{
        val random = Random()
    }

    abstract val name: String

    abstract fun selectLogic(amount: Int): History

    private fun calculateRange(needed: List<Student>): Int {
        val maxE = needed.maxBy { it.selectedAmount }
        val minE = needed.minBy { it.selectedAmount }
        return maxE.selectedAmount - minE.selectedAmount
    }

    private fun calculateAvailableSelectStudents(): MutableList<Student> {
        val tmpStudents = clazz.students
        var availableRange = calculateRange(tmpStudents)
        while (availableRange > MAX_AVAIL_RANGE && tmpStudents.size >= MIN_SELECTION_POOL_AMOUNT) {
            tmpStudents.remove(clazz.students.maxBy { it.selectedAmount })
            availableRange = calculateRange(tmpStudents)
        }
        val average = clazz.students.sumOf { it.selectedAmount } / clazz.students.size
        tmpStudents.forEach {
            if (it.selectedAmount >= average) {
                clazz.students.remove(it)
            }
        }
        return tmpStudents
    }

    fun calculateWeight() {
        // 权重影响因素：抽取次数，上次抽取时间与现在的间隔，
        val available = calculateAvailableSelectStudents()
        val average = available.sumOf { it.selectedAmount } / available.size
        clazz.students.forEach {
            if (it.weight <= 1.0) {
                it.weight  = 1.0
            }
        }
        clazz.students.forEach {
            if (available.contains(it)) {
                it.weight += (average - it.selectedAmount) * 1.7
            } else {
                it.weight += 0.8
            }
            if (it.lastSelectedTime != null && ChronoUnit.DAYS.between(LocalDateTime.now(), it.lastSelectedTime) > 3) {
                it.weight += 1.12.pow(ChronoUnit.DAYS.between(it.lastSelectedTime, LocalDateTime.now()).toDouble())
            } else {
                it.weight += ChronoUnit.DAYS.between(LocalDateTime.now(), LocalDateTime.now()).toDouble() * 1.01
            }
            it.weight += random.nextDouble(1.0, 5.0)
        }
    }

    fun select(amount: Int): History {
        calculateWeight()
        val history = selectLogic(amount)
        history.students.forEach {
            it.lastSelectedTime = history.createTime
            it.selectedAmount ++
        }
        return history
    }
}