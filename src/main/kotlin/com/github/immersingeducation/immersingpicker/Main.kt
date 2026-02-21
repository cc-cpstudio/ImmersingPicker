package com.github.immersingeducation.immersingpicker

import com.github.immersingeducation.immersingpicker.data.clazz.PeriodicalClazzStorageUtils
import com.github.immersingeducation.immersingpicker.launch.ImmersingPicker
import kotlinx.coroutines.runBlocking
import tornadofx.*

fun main(args: Array<String>) = runBlocking {
    PeriodicalClazzStorageUtils.start()
    launch<ImmersingPicker>()
}