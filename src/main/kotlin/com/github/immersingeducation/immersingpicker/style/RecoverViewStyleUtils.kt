package com.github.immersingeducation.immersingpicker.style

import javafx.geometry.Pos
import javafx.scene.paint.Color
import javafx.scene.text.FontWeight
import tornadofx.*

class RecoverViewStyleUtils: Stylesheet() {
    companion object {
        val normalButton by cssclass()
        val dangerousButton by cssclass()
    }

    init {
        normalButton {
            maxWidth = Int.MAX_VALUE.px
            minHeight = 18.px
            alignment = Pos.CENTER_LEFT

            fontSize = 14.px
            fontWeight = FontWeight.LIGHT

            backgroundColor = multi(Color.WHITE)
            borderColor = multi(box(Color.LIGHTGRAY))
            borderWidth = multi(box(1.px))
            borderRadius = multi(box(5.px))

            and(hover) {
                backgroundColor = multi(Color.LIGHTGRAY)
                borderColor = multi(box(Color.GRAY))
            }
        }

        dangerousButton {
            maxWidth = Int.MAX_VALUE.px
            minHeight = 18.px
            alignment = Pos.CENTER_LEFT

            fontSize = 14.px
            fontWeight = FontWeight.LIGHT

            backgroundColor = multi(Color.WHITE)
            borderColor = multi(box(Color.LIGHTGRAY))
            borderWidth = multi(box(1.px))
            borderRadius = multi(box(5.px))

            and(hover) {
                backgroundColor = multi(Color.RED)
                borderColor = multi(box(Color.DARKRED))
            }
        }
    }
}