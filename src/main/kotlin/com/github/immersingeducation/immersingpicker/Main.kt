package com.github.immersingeducation.immersingpicker

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.data.clazz.ClazzStorageUtils
import com.github.immersingeducation.immersingpicker.data.clazz.PeriodicalClazzStorageUtils
import com.github.immersingeducation.immersingpicker.launch.ImmersingPicker
import com.github.immersingeducation.immersingpicker.launch.RecoverMode
import com.github.immersingeducation.immersingpicker.launch.recoverReason
import kotlinx.coroutines.runBlocking
import tornadofx.*

fun main(args: Array<String>) = runBlocking {
    ClazzStorageUtils.loadClasses()
    if (Clazz.getCurrentClass() == null) {
        recoverReason = "当前没有可用班级"
        launch<RecoverMode>()
    } else {
        PeriodicalClazzStorageUtils.start()
        launch<ImmersingPicker>()
    }
}