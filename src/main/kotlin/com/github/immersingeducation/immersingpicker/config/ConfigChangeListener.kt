package com.github.immersingeducation.immersingpicker.config

fun interface ConfigChangeListener {
    fun onConfigChanged(old: Any?, new: Any?)
}