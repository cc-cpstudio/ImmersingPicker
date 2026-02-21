package com.github.immersingeducation.immersingpicker.data.config

import com.github.immersingeducation.immersingpicker.config.ConfigGroup
import com.github.immersingeducation.immersingpicker.config.ConfigItem
import com.github.immersingeducation.immersingpicker.tools.BasicUtils
import mu.KotlinLogging
import org.yaml.snakeyaml.Yaml
import java.io.FileInputStream

object ConfigStorageUtils {
    val logger = KotlinLogging.logger {  }

    fun saveConfig() {

    }

    fun loadConfig() {

    }

    fun loadDefaultConfig() {
        val yaml = Yaml()
        logger.trace("成功创建Yaml解析器对象")
        try {
            val gotten = mutableMapOf<String, ConfigGroup>()
            FileInputStream("${BasicUtils.getWorkDirPath()}/ipicker/default_config.yml").use { reader ->
                val map = yaml.load(reader) as Map<String, Map<String, Any>>
                map.forEach { (key, groupMap) ->

                }
            }
        } catch (e: Exception) {
            logger.error("加载默认配置文件时发生异常", e)
        }
    }
}