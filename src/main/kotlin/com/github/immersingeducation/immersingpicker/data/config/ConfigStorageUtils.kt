package com.github.immersingeducation.immersingpicker.data.config

import com.github.immersingeducation.immersingpicker.config.ConfigGroup
import com.github.immersingeducation.immersingpicker.config.ConfigItem
import com.github.immersingeducation.immersingpicker.config.ConfigUtils
import com.github.immersingeducation.immersingpicker.tools.BasicUtils
import mu.KotlinLogging
import org.yaml.snakeyaml.Yaml
import java.io.FileInputStream
import java.io.FileOutputStream
import java.io.OutputStreamWriter
import java.nio.charset.StandardCharsets

object ConfigStorageUtils {
    val logger = KotlinLogging.logger { }

    fun saveConfig() {
        val configs = ConfigUtils.config ?: throw IllegalArgumentException("未加载配置文件")
        val yaml = Yaml()
        logger.trace("成功创建Yaml解析器对象")
        try {
            val configMap = mutableMapOf<String, Any>()
            
            configs.forEach { (groupKey, group) ->
                val groupMap = mutableMapOf<String, Any>()
                groupMap["name"] = group.name
                
                val configsMap = mutableMapOf<String, Any>()
                group.configs.forEach { (configKey, configItem) ->
                    val configItemMap = mutableMapOf<String, Any>()
                    configItemMap["name"] = configItem.name
                    configItemMap["desc"] = configItem.desc
                    configItemMap["value"] = configItem.value as Any
                    configItemMap["needRestart"] = configItem.needRestart
                    configsMap[configKey] = configItemMap
                }
                
                groupMap["configs"] = configsMap
                configMap[groupKey] = groupMap
            }
            
            val configFile = "${BasicUtils.getWorkDirPath()}/ipicker/config.yml"
            FileOutputStream(configFile).use { outputStream ->
                OutputStreamWriter(outputStream, StandardCharsets.UTF_8).use { writer ->
                    yaml.dump(configMap, writer)
                }
            }
            logger.trace("成功保存配置文件")
        } catch (e: Exception) {
            logger.error("保存配置文件时发生异常", e)
        }
    }

    fun loadConfig() {
        val yaml = Yaml()
        logger.trace("成功创建Yaml解析器对象")
        try {
            val gotten = mutableMapOf<String, ConfigGroup>()
            FileInputStream("${BasicUtils.getWorkDirPath()}/ipicker/config.yml").use { reader ->
                val map = yaml.load(reader) as Map<String, Map<String, Any>>
                map.forEach { (key, groupMap) ->
                    val groupName = groupMap["name"] as String
                    val configsMap = groupMap["configs"] as Map<String, Map<String, Any>>
                    val configs = mutableMapOf<String, ConfigItem>()
                    
                    configsMap.forEach { (configKey, configItemMap) ->
                        val configName = configItemMap["name"] as String
                        val configDesc = configItemMap["desc"] as String
                        val configValue = configItemMap["value"]
                        val needRestart = configItemMap.getOrDefault("needRestart", false) as Boolean
                        
                        configs[configKey] = ConfigItem(
                            name = configName,
                            desc = configDesc,
                            value = configValue,
                            needRestart = needRestart
                        )
                    }
                    
                    gotten[key] = ConfigGroup(
                        name = groupName,
                        configs = configs
                    )
                }
            }
            logger.trace("成功加载配置文件")
            ConfigUtils.config = gotten
        } catch (e: Exception) {
            logger.error("加载配置文件时发生异常", e)
            ConfigUtils.config = null
        }
    }

    fun loadDefaultConfig() {
        val yaml = Yaml()
        logger.trace("成功创建Yaml解析器对象")
        try {
            val gotten = mutableMapOf<String, ConfigGroup>()
            FileInputStream("${BasicUtils.getWorkDirPath()}/ipicker/default_config.yml").use { reader ->
                val map = yaml.load(reader) as Map<String, Map<String, Any>>
                map.forEach { (key, groupMap) ->
                    val groupName = groupMap["name"] as String
                    val configsMap = groupMap["configs"] as Map<String, Map<String, Any>>
                    val configs = mutableMapOf<String, ConfigItem>()
                    
                    configsMap.forEach { (configKey, configItemMap) ->
                        val configName = configItemMap["name"] as String
                        val configDesc = configItemMap["desc"] as String
                        val configValue = configItemMap["value"]
                        val needRestart = configItemMap.getOrDefault("needRestart", false) as Boolean
                        
                        configs[configKey] = ConfigItem(
                            name = configName,
                            desc = configDesc,
                            value = configValue,
                            needRestart = needRestart
                        )
                    }
                    
                    gotten[key] = ConfigGroup(
                        name = groupName,
                        configs = configs
                    )
                }
            }
            logger.trace("成功加载默认配置文件")
            ConfigUtils.defConfig = gotten
        } catch (e: Exception) {
            logger.error("加载默认配置文件时发生异常", e)
            ConfigUtils.defConfig = null
        }
    }
}