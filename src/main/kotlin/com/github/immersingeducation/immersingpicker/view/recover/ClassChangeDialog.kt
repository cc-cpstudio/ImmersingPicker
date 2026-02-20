package com.github.immersingeducation.immersingpicker.view.recover

import com.github.immersingeducation.immersingpicker.config.ConfigUtils
import com.github.immersingeducation.immersingpicker.core.Clazz
import javafx.scene.control.ButtonType
import javafx.scene.control.ComboBox
import javafx.scene.control.Dialog
import javafx.scene.control.ListCell
import javafx.util.Callback

class ClassChangeDialog: Dialog<Clazz>() {
    init {
        title = "切换班级"
        dialogPane.buttonTypes.addAll(
            ButtonType.OK,
            ButtonType.CANCEL
        )

        val changeComboBox = ComboBox<Clazz>().apply {
            items.addAll(Clazz.classes)
            promptText = "请选择班级"
            value = ConfigUtils.getConfig("currentClass")?.value as Clazz?

            cellFactory = Callback {
                object : ListCell<Clazz>() {
                    override fun updateItem(item: Clazz?, empty: Boolean) {
                        super.updateItem(item, empty)
                        text = if (empty || item == null) { null } else { item.name }
                    }
                }
            }

            buttonCell = object : ListCell<Clazz>() {
                override fun updateItem(item: Clazz?, empty: Boolean) {
                    super.updateItem(item, empty)
                    text = if (empty || item == null) { null } else { item.name }
                }
            }
        }

        dialogPane.content = changeComboBox

        resultConverter = Callback {
            when(it) {
                ButtonType.OK -> changeComboBox.value
                else -> null
            }
        }

    }
}