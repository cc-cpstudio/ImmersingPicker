package com.github.immersingeducation.immersingpicker.view.main

import com.github.immersingeducation.immersingpicker.component.SeatGrid
import com.github.immersingeducation.immersingpicker.config.ConfigUtils
import com.github.immersingeducation.immersingpicker.core.Clazz
import javafx.geometry.Pos
import javafx.scene.paint.Color
import tornadofx.*

class MainView: View("ImmersingPicker - 主界面") {
    override val root = borderpane {
        paddingAll = 10.0

        center {
            add(SeatGrid(ConfigUtils.getConfig("currentClass")?.value as Clazz?))
        }

        bottom {
            hbox {
                paddingAll = 10.0
                spacing = 10.0
                alignment = Pos.CENTER

                style {
                    backgroundColor = multi(Color.LIGHTGRAY)
                    backgroundRadius = multi(box(5.px))
                }

                button("-1") {
                    style {
                        minWidth = 40.px
                        minHeight = 40.px
                    }
                }

                button("Select!") {
                    style {
                        minWidth = 100.px
                        minHeight = 40.px
                    }
                }

                button("+1") {
                    style {
                        minWidth = 40.px
                        minHeight = 40.px
                    }
                }
            }
        }
    }
}