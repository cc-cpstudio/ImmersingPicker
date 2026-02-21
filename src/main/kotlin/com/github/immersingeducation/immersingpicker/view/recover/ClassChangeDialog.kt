package com.github.immersingeducation.immersingpicker.view.recover

import com.github.immersingeducation.immersingpicker.config.ConfigUtils
import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.data.clazz.ClazzStorageUtils
import javafx.scene.control.ButtonType
import javafx.scene.control.ComboBox
import javafx.scene.control.Dialog
import javafx.scene.control.ListCell
import javafx.util.Callback
import mu.KotlinLogging

class ClassChangeDialog: Dialog<Int>() {
    companion object {
        val logger = KotlinLogging.logger {}
    }

    init {
        title = "切换班级"
        dialogPane.buttonTypes.addAll(
            ButtonType.OK,
            ButtonType.CANCEL
        )

        val changeComboBox = ComboBox<Int>().apply {
            items.addAll(Clazz.classes.indices.toList())
            promptText = "请选择班级"
            value = Clazz.currentIndex

            cellFactory = Callback {
                object : ListCell<Int>() {
                    override fun updateItem(item: Int?, empty: Boolean) {
                        super.updateItem(item, empty)
                        text = if (empty || item == null) { null } else {
                            Clazz.classes[item].name
                        }
                    }
                }
            }

            buttonCell = object : ListCell<Int>() {
                override fun updateItem(item: Int?, empty: Boolean) {
                    super.updateItem(item, empty)
                    text = if (empty || item == null) { null } else {
                        Clazz.classes[item].name
                    }
                }
            }
        }

        dialogPane.content = changeComboBox

        resultConverter = Callback {
            when(it) {
                ButtonType.OK -> {
                    val selectedIndex = changeComboBox.value
                    selectedIndex?.let {
                        Clazz.currentIndex = it
                    }
                    ClazzStorageUtils.saveClasses()
                    logger.info("成功切换班级为 ${Clazz.classes[selectedIndex!!].name}")
                    selectedIndex
                }
                else -> {
                    logger.info("取消切换班级")
                    null
                }
            }
        }

        logger.info("成功打开班级切换对话框")
    }
}