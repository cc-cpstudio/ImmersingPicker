package com.github.immersingeducation.immersingpicker.config

fun interface ConfigChangeListener {
    suspend fun onConfigChanged(old: Any?, new: Any?)
}