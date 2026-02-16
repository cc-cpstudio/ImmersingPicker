package com.github.immersingeducation.immersingpicker.component

import com.github.immersingeducation.immersingpicker.core.Student
import javafx.scene.control.Alert
import javafx.scene.paint.Color
import tornadofx.*

class Seat(val student: Student): Fragment() {
    override val root = borderpane {
        style {
            minWidth = 30.px
            minHeight = 18.px
            maxWidth = 30.px
            maxHeight = 18.px
            backgroundColor = multi(Color.LIGHTGRAY)
            borderWidth = multi(box(1.px))
            borderColor = multi(box(Color.DARKGRAY))
            borderRadius = multi(box(1.px))
        }

        setOnMouseClicked { event ->
            alert(
                type = Alert.AlertType.INFORMATION,
                header = "学生信息：${student.id}号 ${student.name}",
                content = """
                    学号：${student.id}号
                    姓名：${student.name}
                    座位：${student.seatRow}行，${student.seatColumn}列
                """.trimIndent()
            )
        }

        center {
            hbox(spacing = 10) {
                label("${student.id}")
                label(student.name)
            }
        }
    }
}