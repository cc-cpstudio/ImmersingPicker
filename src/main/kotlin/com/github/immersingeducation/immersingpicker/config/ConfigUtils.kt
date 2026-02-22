package com.github.immersingeducation.immersingpicker.config

import com.github.immersingeducation.immersingpicker.config.enums.SelectedAmountWeightCalculateMode
import kotlinx.coroutines.*
import mu.KotlinLogging

/**
 * 配置工具类，用于管理配置项的加载、获取、设置和监听
 * @author CC想当百大
 * @since v1.0.0.a
 */
object ConfigUtils {
    val logger = KotlinLogging.logger {}
    
    var defConfig: MutableMap<String, ConfigGroup>? = null

    var config: MutableMap<String, ConfigGroup>? = null

    val listeners = mutableMapOf<ConfigItem, MutableList<ConfigChangeListener>>()

    /**
     * 获取指定ID的配置项
     * @param id 配置项的ID
     * @return 配置项对象，如果未找到则返回null
     * @author CC想当百大
     * @since v1.0.0.a
     */
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
            return null
        } catch (e: Exception) {
            logger.error(e) { "获取配置项 $id 时发生未知错误" }
            return null
        }
    }

    /**
     * 设置指定ID的配置项值
     * @param name 配置项的名称
     * @param value 要设置的新值
     * @throws IllegalArgumentException 如果未找到指定名称的配置项或未加载配置文件
     * @author CC想当百大
     * @since v1.0.0.a
     */
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

    /**
     * 检查指定的配置项是否存在
     * @param item 要检查的配置项对象
     * @return 如果配置项存在则返回true，否则返回false
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun itemExists(item: ConfigItem): Boolean {
        var flag = false
        config?.forEach { (_, group) ->
            group.configs.forEach { (_, configItem) ->
                if (configItem == item) {
                    flag = true
                }
            }
        }
        return flag
    }

    /**
     * 注册配置项的监听器
     * @param id 要注册监听器的配置项ID
     * @param listener 要注册的监听器对象
     * @throws IllegalArgumentException 如果未找到指定配置项或未加载配置文件
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun registerListener(id: String, listener: ConfigChangeListener) {
        val item = getConfig(id) ?: throw IllegalArgumentException("未找到配置项：$id")
        logger.debug { "开始注册配置监听器: $id, 监听器: ${listener.javaClass.simpleName}" }
        try {
            if (listeners[item] != null && listeners[item]!!.contains(listener)) {
                logger.debug { "监听器已存在，跳过注册: $id" }
                return
            } else {
                if (listeners[item] == null) {
                    listeners[item] = mutableListOf()
                    logger.debug { "创建监听器列表: $id" }
                } else {
                    listeners[item]!!.add(listener)
                }
                logger.debug { "监听器注册成功: $id, 当前监听器数量: ${listeners[item]!!.size}" }
            }
        } catch (e: IllegalArgumentException)  {
            logger.error { "注册监听器失败: ${e.message}" }
            throw e
        } catch (e: Exception) {
            logger.error(e) { "注册监听器时发生错误: $id" }
            throw e
        }
    }

    /**
     * 注销指定ID的配置项的监听器
     * @param id 配置项的ID
     * @param listener 要注销的监听器对象
     * @throws IllegalArgumentException 如果未找到指定ID的配置项或未加载配置文件
     * @author CC想当百大
     * @since v1.0.0.a
     */
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

    /**
     * 注销指定ID的配置项的所有监听器
     * @param id 配置项的ID
     * @throws IllegalArgumentException 如果未找到指定ID的配置项或未加载配置文件
     * @author CC想当百大
     * @since v1.0.0.a
     */
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

    /**
     * 注销所有配置项的所有监听器
     * @author CC想当百大
     * @since v1.0.0.a
     */
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

    /**
     * 通知指定配置项的所有监听器，配置项的值发生变化
     * @param item 配置项对象
     * @param old 配置项变化前的旧值
     * @param new 配置项变化后的新值
     * @author CC想当百大
     * @since v1.0.0.a
     */
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