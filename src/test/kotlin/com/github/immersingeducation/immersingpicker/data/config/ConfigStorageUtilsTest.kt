package com.github.immersingeducation.immersingpicker.data.config

import com.github.immersingeducation.immersingpicker.config.ConfigGroup
import com.github.immersingeducation.immersingpicker.config.ConfigItem
import mu.KotlinLogging
import org.junit.jupiter.api.AfterEach
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import java.io.File
import org.junit.jupiter.api.Assertions.assertNotNull
import org.junit.jupiter.api.Assertions.assertTrue

class ConfigStorageUtilsTest {

    private val logger = KotlinLogging.logger {  }
    private val testConfigFile = "ipicker/config.yml"

    @BeforeEach
    fun setUp() {
        // 测试前清理可能存在的配置文件
        val file = File(testConfigFile)
        if (file.exists()) {
            file.delete()
            logger.trace("清理测试配置文件")
        }
    }

    @AfterEach
    fun tearDown() {
        // 测试后清理配置文件
        val file = File(testConfigFile)
        if (file.exists()) {
            file.delete()
            logger.trace("清理测试配置文件")
        }
    }

    @Test
    fun testLoadDefaultConfigShouldReturnNonNullMap() {
        val config = ConfigStorageUtils.loadDefaultConfig()
        assertNotNull(config, "加载默认配置应该返回非空映射")
        config?.let {
            assertTrue(it.isNotEmpty(), "默认配置映射应该包含配置组")
        }
        logger.trace("成功测试加载默认配置")
    }

    @Test
    fun testLoadConfigShouldReturnNullWhenConfigFileNotExists() {
        val config = ConfigStorageUtils.loadConfig()
        assertTrue(config == null, "当配置文件不存在时，加载配置应该返回null")
        logger.trace("成功测试加载不存在的配置文件")
    }

    @Test
    fun testSaveConfigAndLoadConfigShouldWorkCorrectly() {
        // 首先加载默认配置
        val defaultConfig = ConfigStorageUtils.loadDefaultConfig()
        assertNotNull(defaultConfig, "加载默认配置应该返回非空映射")

        // 修改配置
        val modifiedConfig = mutableMapOf<String, ConfigGroup>()
        defaultConfig?.forEach { (key, group) ->
            val modifiedConfigs = mutableMapOf<String, ConfigItem>()
            group.configs.forEach { (configKey, item) ->
                // 创建新的ConfigItem实例，避免并发修改问题
                val newValue = if (item.value is Boolean) {
                    !(item.value as Boolean)
                } else {
                    item.value
                }
                modifiedConfigs[configKey] = ConfigItem(
                    name = item.name,
                    desc = item.desc,
                    value = newValue,
                    needRestart = item.needRestart
                )
            }
            // 创建新的ConfigGroup实例
            modifiedConfig[key] = ConfigGroup(
                name = group.name,
                configs = modifiedConfigs
            )
        }

        // 保存配置
        ConfigStorageUtils.saveConfig(modifiedConfig)

        // 验证配置文件是否存在
        val configFile = File(testConfigFile)
        assertTrue(configFile.exists(), "配置文件应该被成功创建")

        // 加载配置
        val loadedConfig = ConfigStorageUtils.loadConfig()
        assertNotNull(loadedConfig, "加载配置应该返回非空映射")

        // 验证加载的配置是否与修改后的配置一致
        loadedConfig?.let {
            assertTrue(it.size == modifiedConfig.size, "配置组数量应该一致")
        }
        logger.trace("成功测试保存和加载配置")
    }
}
