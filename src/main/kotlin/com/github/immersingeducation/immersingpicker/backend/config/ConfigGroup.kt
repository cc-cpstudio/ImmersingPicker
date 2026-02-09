package com.github.immersingeducation.immersingpicker.backend.config

data class ConfigGroup(
    val name: String,
    val configs: Map<String, ConfigItem<out Any>>
)