package com.github.immersingeducation.immersingpicker.config

import com.github.immersingeducation.immersingpicker.config.enums.SelectedAmountWeightCalculateMode
import kotlinx.coroutines.*
import mu.KotlinLogging

object ConfigUtils {
    val logger = KotlinLogging.logger {}
    
    var defConfig: MutableMap<String, ConfigGroup>? = null

    var config: MutableMap<String, ConfigGroup>? = null

    val listeners = mutableMapOf<ConfigItem, MutableList<ConfigChangeListener>>()

    fun getConfig(id: String): ConfigItem? {
        logger.debug { "开始获取配置项: $id" }
        try {
            config?.forEach { (_, group) ->
                group.configs.forEach { (nameOfItem, item) ->
                    if (nameOfItem == id) {
                        logger.debug { "找到配置项: $id, 值: ${item.value}" }
                        return item
                    }
                }
            } ?: throw IllegalArgumentException("未加载配置文件")
            logger.debug { "未找到配置项: $id" }
            return null
        } catch (e: IllegalArgumentException) {
            logger.error { "获取配置项失败: ${e.message}" }
            throw e
        } catch (e: Exception) {
            logger.error(e) { "获取配置项时发生未知错误: $id" }
            throw e
        }
    }

    fun setConfig(name: String, value: Any?) {
        logger.debug { "开始设置配置项: $name, 值: $value" }
        try {
            var flag = false
            config?.forEach { (_, group) ->
                group.configs.forEach { (nameOfItem, item) ->
                    if (nameOfItem == name) {
                        flag = true
                        val oldValue = item.value
                        item.value = value
                        logger.debug { "配置项已更新: $name, 旧值: $oldValue, 新值: $value" }
                        notifyListeners(item, oldValue, value)
                        if (item.needRestart) {
                            logger.warn { "配置项 $name 需要重启应用" }
                            TODO("需要重启")
                        }
                    }
                }
            } ?: throw IllegalArgumentException("未加载配置文件")
            if (!flag) {
                throw IllegalArgumentException("未找到配置项：$name")
            }
            logger.debug { "配置项设置完成: $name" }
        } catch (e: IllegalArgumentException) {
            logger.error { "设置配置项失败: ${e.message}" }
            throw e
        } catch (e: Exception) {
            logger.error(e) { "设置配置项时发生未知错误: $name, 值: $value" }
            throw e
        }
    }

    fun registerListener(item: ConfigItem, listener: ConfigChangeListener) {
        logger.debug { "开始注册配置监听器: ${item.name}, 监听器: ${listener.javaClass.simpleName}" }
        try {
            if (listeners[item] != null && listeners[item]!!.contains(listener)) {
                logger.debug { "监听器已存在，跳过注册: ${item.name}" }
                return
            } else {
                if (listeners[item] == null) {
                    listeners[item] = mutableListOf()
                    logger.debug { "创建监听器列表: ${item.name}" }
                }
                listeners[item]!!.add(listener)
                logger.debug { "监听器注册成功: ${item.name}, 当前监听器数量: ${listeners[item]!!.size}" }
            }
        } catch (e: Exception) {
            logger.error(e) { "注册监听器时发生错误: ${item.name}" }
            throw e
        }
    }

    fun unregisterListener(id: String, listener: ConfigChangeListener) {
        logger.debug { "开始注销配置监听器: $id, 监听器: ${listener.javaClass.simpleName}" }
        try {
            val item = getConfig(id)
            if (listeners[item] == null) {
                logger.error { "未找到配置项的监听器列表: $id" }
                throw IllegalArgumentException("未找到配置项：$id")
            } else {
                if (listeners[item]!!.contains(listener)) {
                    listeners[item]!!.remove(listener)
                    logger.debug { "监听器注销成功: $id, 当前监听器数量: ${listeners[item]!!.size}" }
                } else {
                    logger.error { "未找到指定监听器: $id, 监听器: ${listener.javaClass.simpleName}" }
                    throw IllegalArgumentException("未找到监听器：$listener")
                }
            }
        } catch (e: IllegalArgumentException) {
            logger.error { "注销监听器失败: ${e.message}" }
            throw e
        } catch (e: Exception) {
            logger.error(e) { "注销监听器时发生未知错误: $id" }
            throw e
        }
    }

    fun unregisterAllListeners(id: String) {
        logger.debug { "开始注销所有配置监听器: $id" }
        try {
            val item = getConfig(id)
            val listenerCount = listeners[item]?.size ?: 0
            listeners[item]?.clear()
            logger.debug { "注销所有监听器成功: $id, 共注销 $listenerCount 个监听器" }
        } catch (e: Exception) {
            logger.error(e) { "注销所有监听器时发生错误: $id" }
            throw e
        }
    }

    fun unregisterAllListeners() {
        logger.debug { "开始注销所有配置监听器" }
        try {
            val totalListenerCount = listeners.values.sumOf { it.size }
            listeners.clear()
            logger.debug { "注销所有监听器成功，共注销 $totalListenerCount 个监听器" }
        } catch (e: Exception) {
            logger.error(e) { "注销所有监听器时发生错误" }
            throw e
        }
    }

    private fun notifyListeners(item: ConfigItem, old: Any?, new: Any?) {
        logger.debug { "开始通知配置监听器: ${item.name}, 旧值: $old, 新值: $new" }
        try {
            val listenerList = listeners[item]
            if (listenerList != null && listenerList.isNotEmpty()) {
                logger.debug { "通知监听器数量: ${listenerList.size}, 配置项: ${item.name}" }
                listenerList.forEach {
                    try {
                        it.onConfigChanged(old, new)
                        logger.debug { "监听器通知成功: ${it.javaClass.simpleName}, 配置项: ${item.name}" }
                    } catch (e: Exception) {
                        logger.error(e) { "监听器处理异常: ${it.javaClass.simpleName}, 配置项: ${item.name}" }
                    }
                }
            } else {
                logger.debug { "无监听器需要通知: ${item.name}" }
            }
        } catch (e: Exception) {
            logger.error(e) { "通知监听器时发生错误: ${item.name}" }
        }
    }
}