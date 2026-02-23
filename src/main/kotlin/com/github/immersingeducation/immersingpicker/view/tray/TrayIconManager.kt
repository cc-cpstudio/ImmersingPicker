package com.github.immersingeducation.immersingpicker.view.tray

import com.dustinredmond.fxtrayicon.FXTrayIcon
import com.github.immersingeducation.immersingpicker.launch.AppUtils
import javafx.application.Platform
import javafx.scene.control.MenuItem
import javafx.stage.Stage

class TrayIconManager(private val stage: Stage) {
    private lateinit var trayIcon: FXTrayIcon

    fun initTray() {
        try {
            trayIcon = FXTrayIcon(
                stage,
                javaClass.getResource("/view/tray/tray_icon.png")
            )
            addDefaultMenuItems()
            trayIcon.show()
        } catch (e: Exception) {

        }
    }

    fun addDefaultMenuItems() {
        trayIcon.addMenuItems(
            MenuItem("显示主窗口").apply {
                setOnAction {
                    Platform.runLater {
                        stage.show()
                        stage.toFront()
                    }
                }
            },
            MenuItem("重新启动").apply {
                setOnAction {
                    Platform.runLater {
                        trayIcon.hide()
                        AppUtils.restart()
                    }
                }
            },
            MenuItem("退出程序").apply {
                setOnAction {
                    Platform.runLater {
                        trayIcon.hide()
                        AppUtils.exit()
                    }
                }
            }
        )
    }
}