package com.github.immersingeducation.immersingpicker.config

data class ConfigItem (
    val name: String,
    val desc: String,
    val def: Any,
    var value: Any?
) {

}