package com.github.immersingeducation.immersingpicker.config

/**
 * 配置组类，用于组织相关的配置项
 * @param name 配置组的名称
 * @param configs 配置项的映射，键为配置项的名称，值为配置项对象
 * @author CC想当百大
 * @since v1.0.0.a
 */
data class ConfigGroup(
    val name: String,
    val configs: Map<String, ConfigItem>
)