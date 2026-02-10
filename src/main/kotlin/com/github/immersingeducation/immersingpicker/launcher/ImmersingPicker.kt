package com.github.immersingeducation.immersingpicker.launcher

import com.github.immersingeducation.immersingpicker.view.main.MainView
import javafx.stage.Stage
import tornadofx.*

class ImmersingPicker: App(MainView::class) {
    init {
        reloadStylesheetsOnFocus()
    }

    override fun start(stage: Stage) {
        super.start(stage)
        stage.apply {
            width = 800.0
            height = 600.0
        }
    }
}