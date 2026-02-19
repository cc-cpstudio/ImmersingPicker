package com.github.immersingeducation.immersingpicker.launch

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
            minWidth = 800.0
            minHeight = 600.0
        }
    }
}