package com.github.immersingeducation.immersingpicker

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.data.clazz.ClazzStorageUtils
import com.github.immersingeducation.immersingpicker.data.GlobalStorageUtils
import com.github.immersingeducation.immersingpicker.launch.ImmersingPicker
import com.github.immersingeducation.immersingpicker.launch.RecoverMode
import com.github.immersingeducation.immersingpicker.launch.recoverReason
import kotlinx.coroutines.runBlocking
import tornadofx.*

fun main(args: Array<String>) = runBlocking {
    GlobalStorageUtils.loadData()
    if (Clazz.getCurrentClazz() == null) {
        recoverReason = "当前没有可用班级"
        launch<RecoverMode>()
    } else {
        GlobalStorageUtils.start()
        launch<ImmersingPicker>()
    }
}