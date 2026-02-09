package com.github.immersingeducation.immersingpicker.config

data class ConfigItem<T> (
    val name: String,
    val desc: String,
    val def: T,
    var value: T?
) {

}