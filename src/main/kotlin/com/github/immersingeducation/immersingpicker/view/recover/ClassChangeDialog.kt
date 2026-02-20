package com.github.immersingeducation.immersingpicker.view.recover

import com.github.immersingeducation.immersingpicker.config.ConfigUtils
import com.github.immersingeducation.immersingpicker.core.ClassNGrade
import javafx.scene.control.ButtonType
import javafx.scene.control.ComboBox
import javafx.scene.control.Dialog
import javafx.scene.control.Label
import javafx.scene.control.ListCell
import javafx.util.Callback

class ClassChangeDialog: Dialog<ClassNGrade>() {
    init {
        title = "切换班级"
        dialogPane.buttonTypes.addAll(
            ButtonType.OK,
            ButtonType.CANCEL
        )

        val changeComboBox = ComboBox<ClassNGrade>().apply {
            items.addAll(ClassNGrade.classes)
            promptText = "请选择班级"
            value = ConfigUtils.getConfig("currentClass")?.value as ClassNGrade?

            cellFactory = Callback {
                object : ListCell<ClassNGrade>() {
                    override fun updateItem(item: ClassNGrade?, empty: Boolean) {
                        super.updateItem(item, empty)
                        text = if (empty || item == null) { null } else { item.name }
                    }
                }
            }

            buttonCell = object : ListCell<ClassNGrade>() {
                override fun updateItem(item: ClassNGrade?, empty: Boolean) {
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