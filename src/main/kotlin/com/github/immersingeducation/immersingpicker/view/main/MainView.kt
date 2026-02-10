package com.github.immersingeducation.immersingpicker.view.main

import javafx.animation.FadeTransition
import javafx.animation.SequentialTransition
import javafx.geometry.Pos
import javafx.scene.Parent
import javafx.scene.layout.StackPane
import javafx.util.Duration
import tornadofx.*

class MainView: View("ImmersingPicker - 主界面") {
    override val root = borderpane {
        center {
            label("Hello world!")
        }
        bottom {
            hbox {
                paddingAll = 10.0
                spacing = 10.0
                alignment = Pos.CENTER

                button("I'm Button!")
            }
        }
    }
}