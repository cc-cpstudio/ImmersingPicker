package com.github.immersingeducation.immersingpicker.launch

import com.github.immersingeducation.immersingpicker.data.DataPersistence
import com.github.immersingeducation.immersingpicker.exception.NoAvailableClass
import com.github.immersingeducation.immersingpicker.view.main.MainView
import com.github.immersingeducation.immersingpicker.view.recover.RecoverModeView
import javafx.stage.Stage
import tornadofx.*

class RecoverMode: App(RecoverModeView::class) {
    override fun start(stage: Stage) {
        super.start(stage)
        stage.apply {
            minWidth = 300.0
        }
    }
}

class ImmersingPicker: App(MainView::class) {
    override fun start(stage: Stage) {
        try {
            super.start(stage)
            stage.apply {
                minWidth = 800.0
                minHeight = 600.0
            }
            
            // 添加关闭钩子，在应用程序退出时保存数据
            Runtime.getRuntime().addShutdownHook(Thread {
                DataPersistence.saveClasses()
            })
        } catch(e: NoAvailableClass) {
            launch<RecoverMode>()
        }
    }
}