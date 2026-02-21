package com.github.immersingeducation.immersingpicker.component

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.Student
import com.github.immersingeducation.immersingpicker.exception.NoAvailableClass
import javafx.geometry.Insets
import javafx.scene.layout.GridPane
import mu.KotlinLogging

class SeatGrid(val clazz: Clazz?): GridPane() {
    val seats = mutableListOf<Seat>()

    companion object {
        val logger = KotlinLogging.logger {}
    }

    init {
        if (clazz == null) {
            throw NoAvailableClass()
        }
        hgap = 5.0; vgap = 5.0
        padding = Insets(5.0)
        refresh()
        logger.debug("成功创建 SeatGrid")
    }

    private fun addStudent(student: Student) {
        if (student.seatRow >= 0 && student.seatColumn > 0) {
            val s = Seat(student)
            seats.add(s)
            add(s.root, student.seatColumn - 1, student.seatRow)
            logger.debug("成功添加学生 ${student.id} 到座位 ${student.seatRow}行，${student.seatColumn}列")
        } else {
            logger.warn("学生 ${student.id} 没有指定座位或座位不合法，未添加")
        }
    }

    fun refresh() {
        children.clear()
        seats.clear()
        logger.trace("清空成功，开始重新添加学生列表")
        clazz?.students?.forEach {
            addStudent(it)
        }
        logger.debug("成功刷新 SeatGrid")
    }

    constructor(): this(Clazz.getCurrentClazz())
}