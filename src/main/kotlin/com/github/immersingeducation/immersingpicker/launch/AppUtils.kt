package com.github.immersingeducation.immersingpicker.launch

import kotlin.system.exitProcess

object AppUtils {
    fun exit() {
        TODO("我要数据持久化")
        exitProcess(ExitCode.MANUAL_EXIT)
    }

    fun restart() {
        TODO("我要获取程序路径")
        exitProcess(ExitCode.MANUAL_RESTART)
    }
}