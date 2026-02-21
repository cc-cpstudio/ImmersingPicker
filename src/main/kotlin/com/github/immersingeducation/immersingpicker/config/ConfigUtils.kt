package com.github.immersingeducation.immersingpicker.config

import com.github.immersingeducation.immersingpicker.config.enums.SelectedAmountWeightCalculateMode
import kotlinx.coroutines.*

object ConfigUtils {
    var defConfig: MutableMap<String, ConfigGroup>? = null

    var config: MutableMap<String, ConfigGroup>? = null
    // TODO 数据持久化

    val listeners = mutableMapOf<ConfigItem, MutableList<ConfigChangeListener>>()

    fun getConfig(id: String): ConfigItem? {
        config?.forEach { (_, group) ->
            group.configs.forEach { (nameOfItem, item) ->
                if (nameOfItem == id) {
                    return item
                }
            }
        } ?: throw IllegalArgumentException("未加载配置文件")
        return null
    }

    fun setConfig(name: String, value: Any?) {
        var flag = false
        config?.forEach { (_, group) ->
            group.configs.forEach { (nameOfItem, item) ->
                if (nameOfItem == name) {
                    flag = true
                    item.value = value
                    if (item.needRestart) {
                        TODO("需要重启")
                    }
                }
            }
        } ?: throw IllegalArgumentException("未加载配置文件")
        if (!flag) {
            throw IllegalArgumentException("未找到配置项：$name")
        }
    }

    fun registerListener(item: ConfigItem, listener: ConfigChangeListener) {
        if (listeners[item] != null && listeners[item]!!.contains(listener)) {
            return
        } else {
            if (listeners[item] == null) {
                listeners[item] = mutableListOf()
            } else {
                listeners[item]!!.add(listener)
            }
        }
    }

    fun unregisterListener(id: String, listener: ConfigChangeListener) {
        val item = getConfig(id)
        if (listeners[item] == null) {
            throw IllegalArgumentException("未找到配置项：$id")
        } else {
            if (listeners[item]!!.contains(listener)) {
                listeners[item]!!.remove(listener)
            } else {
                throw IllegalArgumentException("未找到监听器：$listener")
            }
        }
    }

    fun unregisterAllListeners(id: String) {
        listeners[getConfig(id)]?.clear()
    }

    fun unregisterAllListeners() {
        listeners.clear()
    }

    private fun notifyListeners(item: ConfigItem, old: Any?, new: Any?) {
        listeners[item]?.forEach {
            it.onConfigChanged(old, new)
        }
    }
}