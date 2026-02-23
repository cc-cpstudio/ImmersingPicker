package com.github.immersingeducation.immersingpicker.launch

import com.dustinredmond.fxtrayicon.FXTrayIcon
import com.github.immersingeducation.immersingpicker.view.main.MainView
import com.github.immersingeducation.immersingpicker.view.recover.RecoverModeView
import javafx.application.Platform
import javafx.scene.control.Alert
import javafx.stage.Stage
import tornadofx.*

var recoverReason = "null"

class RecoverMode: App(RecoverModeView::class) {
    override fun start(stage: Stage) {
        super.start(stage)
        stage.apply {
            minWidth = 300.0
        }

        Alert(Alert.AlertType.INFORMATION).apply {
            title = "ImmersingPicker 恢复模式"
            headerText = "您现在处于恢复模式"
            contentText = "进入原因：$recoverReason"
        } .showAndWait()
    }
}

class ImmersingPicker: App(MainView::class) {
    override fun start(stage: Stage) {
        super.start(stage)
        stage.apply {
            minWidth = 800.0
            minHeight = 600.0
        }

        val trayIcon = FXTrayIcon.Builder(stage, javaClass.getResource("/icon.png"))
            .menuItem("显示主窗口") { _ ->
                Platform.runLater {
                    stage.show()
                    stage.toFront()
                }
            }
            .menuItem("重新启动") { _ ->

            }
            .menuItem("退出") { _ ->

            }
            .show().build()
    }
}