package com.github.immersingeducation.immersingpicker.config

/**
 * 配置项类，用于表示一个具体的配置项
 * @param name 配置项的名称
 * @param desc 配置项的描述
 * @param value 配置项的当前值
 * @param needRestart 是否需要重启应用才能生效
 * @author CC想当百大
 * @since v1.0.0.a
 */
data class ConfigItem (
    val name: String,
    val desc: String,
    var value: Any?,
    val needRestart: Boolean = false
)