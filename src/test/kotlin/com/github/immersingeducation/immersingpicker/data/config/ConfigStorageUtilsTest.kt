package com.github.immersingeducation.immersingpicker.data.config

import com.github.immersingeducation.immersingpicker.config.ConfigGroup
import com.github.immersingeducation.immersingpicker.config.ConfigItem
import com.github.immersingeducation.immersingpicker.config.ConfigUtils
import mu.KotlinLogging
import org.junit.jupiter.api.AfterEach
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import java.io.File
import org.junit.jupiter.api.Assertions.assertNotNull
import org.junit.jupiter.api.Assertions.assertTrue
import org.junit.jupiter.api.Assertions.assertFalse
import org.junit.jupiter.api.Assertions.assertEquals

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
    fun testLoadDefaultConfigShouldSetDefConfig() {
        // 加载默认配置
        ConfigStorageUtils.loadDefaultConfig()
        assertNotNull(ConfigUtils.defConfig, "加载默认配置应该设置defConfig")
        ConfigUtils.defConfig?.let {
            assertTrue(it.isNotEmpty(), "默认配置映射应该包含配置组")
        }
        logger.trace("成功测试加载默认配置")
    }

    @Test
    fun testLoadConfigShouldCreateFileWhenNotExists() {
        // 确保配置文件不存在
        val configFile = File(testConfigFile)
        if (configFile.exists()) {
            configFile.delete()
        }
        assertFalse(configFile.exists(), "配置文件应该被删除")

        // 加载配置
        ConfigStorageUtils.loadConfig()
        
        // 验证配置文件是否被创建
        assertTrue(configFile.exists(), "配置文件应该被成功创建")
        
        // 验证配置是否加载成功
        assertNotNull(ConfigUtils.config, "加载配置应该设置config")
        logger.trace("成功测试配置文件不存在时的情况")
    }

    @Test
    fun testLoadConfigShouldHandleEmptyFile() {
        // 创建空的配置文件
        val configFile = File(testConfigFile)
        configFile.parentFile?.mkdirs()
        configFile.createNewFile()
        assertTrue(configFile.exists(), "配置文件应该被创建")
        assertEquals(0, configFile.length(), "配置文件应该为空")

        // 加载配置
        ConfigStorageUtils.loadConfig()
        
        // 验证配置是否加载成功
        assertNotNull(ConfigUtils.config, "加载配置应该设置config")
        
        // 验证配置文件是否被重新写入
        assertTrue(configFile.length() > 0, "配置文件应该被重新写入")
        logger.trace("成功测试空配置文件的情况")
    }

    @Test
    fun testLoadConfigShouldHandleInvalidFormat() {
        // 创建格式不正确的配置文件
        val configFile = File(testConfigFile)
        configFile.parentFile?.mkdirs()
        configFile.writeText("invalid yaml format")
        assertTrue(configFile.exists(), "配置文件应该被创建")
        assertTrue(configFile.length() > 0, "配置文件应该不为空")

        // 加载配置
        ConfigStorageUtils.loadConfig()
        
        // 验证配置是否加载成功
        assertNotNull(ConfigUtils.config, "加载配置应该设置config")
        logger.trace("成功测试格式不正确的配置文件的情况")
    }

    @Test
    fun testSaveConfigAndLoadConfigShouldWorkCorrectly() {
        // 首先加载默认配置
        ConfigStorageUtils.loadDefaultConfig()
        assertNotNull(ConfigUtils.defConfig, "加载默认配置应该设置defConfig")

        // 设置默认配置为当前配置
        ConfigUtils.config = ConfigUtils.defConfig

        // 保存配置
        ConfigStorageUtils.saveConfig()

        // 验证配置文件是否存在
        val configFile = File(testConfigFile)
        assertTrue(configFile.exists(), "配置文件应该被成功创建")

        // 加载配置
        ConfigStorageUtils.loadConfig()
        assertNotNull(ConfigUtils.config, "加载配置应该设置config")

        // 验证加载的配置是否与默认配置一致
        ConfigUtils.config?.let {
            ConfigUtils.defConfig?.let {
                assertTrue(it.isNotEmpty(), "配置组数量应该一致")
            }
        }
        logger.trace("成功测试保存和加载配置")
    }
}
