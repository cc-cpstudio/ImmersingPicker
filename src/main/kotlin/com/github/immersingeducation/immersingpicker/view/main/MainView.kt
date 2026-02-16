package com.github.immersingeducation.immersingpicker.view.main

import javafx.animation.FadeTransition
import javafx.animation.SequentialTransition
import javafx.geometry.Pos
import javafx.scene.Parent
import javafx.scene.layout.StackPane
import javafx.scene.paint.Color
import javafx.util.Duration
import tornadofx.*

class MainView: View("ImmersingPicker - 主界面") {
    override val root = borderpane {
        paddingAll = 10.0

        center {
            gridpane {
                hgap = 5.0
                vgap = 5.0
            }
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

                button("I'm Button!") {
                    style {
                        minWidth = 100.px
                        minHeight = 40.px
                    }
                }
            }
        }
    }
}