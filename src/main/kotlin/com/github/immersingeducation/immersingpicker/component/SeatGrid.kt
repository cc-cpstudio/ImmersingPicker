package com.github.immersingeducation.immersingpicker.component

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.Student
import com.github.immersingeducation.immersingpicker.exception.NoAvailableClass
import javafx.geometry.Insets
import javafx.scene.layout.GridPane
import mu.KotlinLogging

/**
 * 座位网格组件，用于按每位学生的座位行列数显示所有 Seat 组件
 * @param clazz 班级对象，用于获取班级中的所有学生
 * @author CC想当百大
 * @since v1.0.0.a
 */
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

    /**
     * 添加学生到座位网格中，根据学生的座位行列数显示对应座位
     * @param student 要添加的学生对象
     * @author CC想当百大
     * @since v1.0.0.a
     */
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

    /**
     * 刷新座位网格，根据班级中的学生列表重新添加座位组件
     * @author CC想当百大
     * @since v1.0.0.a
     */
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