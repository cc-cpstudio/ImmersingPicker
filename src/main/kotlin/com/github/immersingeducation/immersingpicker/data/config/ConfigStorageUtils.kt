package com.github.immersingeducation.immersingpicker.data.config

import com.github.immersingeducation.immersingpicker.config.ConfigGroup
import com.github.immersingeducation.immersingpicker.config.ConfigItem
import com.github.immersingeducation.immersingpicker.config.ConfigUtils
import com.github.immersingeducation.immersingpicker.tools.BasicUtils
import mu.KotlinLogging
import org.yaml.snakeyaml.Yaml
import java.io.File
import java.io.FileInputStream
import java.io.FileOutputStream
import java.io.OutputStreamWriter
import java.nio.charset.StandardCharsets

/**
 * 配置文件存储工具类
 * @author CC想当百大
 * @since v1.0.0.a
 */
object ConfigStorageUtils {
    val logger = KotlinLogging.logger { }

    /**
     * 保存配置文件到磁盘
     * @author CC想当百大
     * @since v1.0.0.a
     */
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
            logger.info("成功保存配置文件：$configFile")
        } catch (e: Exception) {
            logger.error("保存配置文件时发生异常", e)
        }
    }

    /**
     * 保存默认配置文件到磁盘
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun saveDefaultConfig() {
        val configs = ConfigUtils.defConfig ?: throw IllegalArgumentException("未加载配置文件")
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

            val configFile = "${BasicUtils.getWorkDirPath()}/ipicker/default_config.yml"
            FileOutputStream(configFile).use { outputStream ->
                OutputStreamWriter(outputStream, StandardCharsets.UTF_8).use { writer ->
                    yaml.dump(configMap, writer)
                }
            }
            logger.info("成功保存默认配置文件：$configFile")
        } catch (e: Exception) {
            logger.error("保存默认配置文件时发生异常", e)
        }
    }

    /**
     * 从磁盘加载配置文件
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun loadConfig() {
        val yaml = Yaml()
        logger.trace("成功创建Yaml解析器对象")
        try {
            val configFile = "${BasicUtils.getWorkDirPath()}/ipicker/config.yml"
            val file = File(configFile)
            
            // 检查文件是否存在或为空
            if (!file.exists() || file.length() == 0L) {
                logger.warn("配置文件不存在或为空，将使用默认配置并重新创建配置文件")
                resetToDefaultConfig()
                return
            }
            
            val gotten = mutableMapOf<String, ConfigGroup>()
            FileInputStream(configFile).use { reader ->
                val map = yaml.load(reader) as? Map<String, Map<String, Any>>
                
                // 检查解析结果是否有效
                if (map == null) {
                    logger.warn("配置文件格式不正确，将使用默认配置并重新创建配置文件")
                    resetToDefaultConfig()
                    return
                }
                
                map.forEach { (key, groupMap) ->
                    try {
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
                    } catch (e: Exception) {
                        logger.error("解析配置组 $key 时发生异常", e)
                        logger.warn("配置文件格式不正确，将使用默认配置并重新创建配置文件")
                        resetToDefaultConfig()
                        return
                    }
                }
            }
            logger.info("成功加载配置文件")
            ConfigUtils.config = gotten
        } catch (e: Exception) {
            logger.error("加载配置文件时发生异常", e)
            logger.warn("将使用默认配置并重新创建配置文件")
            resetToDefaultConfig()
        }
    }

    /**
     * 重置为默认配置并保存
     * @author CC想当百大
     * @since v1.0.0.a
     */
    private fun resetToDefaultConfig() {
        // 加载默认配置
        loadDefaultConfig()
        // 检查默认配置是否加载成功
        if (ConfigUtils.defConfig != null) {
            // 将默认配置设置为当前配置
            ConfigUtils.config = ConfigUtils.defConfig
            // 保存当前配置
            saveConfig()
            logger.info("成功重置为默认配置并保存")
        } else {
            logger.error("加载默认配置失败，无法重置为默认配置")
        }
    }

    /**
     * 从默认配置文件加载配置
     * @author CC想当百大
     * @since v1.0.0.a
     */
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
            logger.info("成功加载默认配置文件")
            ConfigUtils.defConfig = gotten
        } catch (e: Exception) {
            logger.error("加载默认配置文件时发生异常", e)
        }
    }
}