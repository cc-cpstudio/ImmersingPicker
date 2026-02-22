package com.github.immersingeducation.immersingpicker.component

import com.github.immersingeducation.immersingpicker.core.Student
import javafx.geometry.Pos
import javafx.scene.control.Alert
import javafx.scene.paint.Color
import tornadofx.*

/**
 * 座位组件，用于以座位形式显示学生信息
 * @param student 座位组件对应的学生对象
 * @author CC想当百大
 * @since v1.0.0.a
 */
class Seat(val student: Student): Fragment() {
    override val root = borderpane {
        style {
            minWidth = 60.px
            minHeight = 36.px
            maxWidth = 60.px
            maxHeight = 36.px
            backgroundColor = multi(Color.LIGHTGRAY)
            backgroundRadius = multi(box(4.px))
            borderWidth = multi(box(2.px))
            borderColor = multi(box(Color.DARKGRAY))
            borderRadius = multi(box(4.px))
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
            hbox(spacing = 5) {
                alignment = Pos.CENTER

                label("${student.id}")
                label(student.name)
            }
        }
    }

    /**
     * 座位是否被选中，更改时会同步更新座位的样式
     */
    var selected: Boolean = false
        set(value) {
            if (field != value) {
                field = value
                root.style {
                    if (value) {
                        minWidth = 60.px
                        minHeight = 36.px
                        maxWidth = 60.px
                        maxHeight = 36.px
                        backgroundColor = multi(Color.LIGHTGREEN)
                        backgroundRadius = multi(box(4.px))
                        borderWidth = multi(box(2.px))
                        borderColor = multi(box(Color.GREEN))
                        borderRadius = multi(box(4.px))
                    } else {
                        minWidth = 60.px
                        minHeight = 36.px
                        maxWidth = 60.px
                        maxHeight = 36.px
                        backgroundColor = multi(Color.LIGHTGRAY)
                        backgroundRadius = multi(box(4.px))
                        borderWidth = multi(box(2.px))
                        borderColor = multi(box(Color.DARKGRAY))
                        borderRadius = multi(box(4.px))
                    }
                }
            }
        }
}