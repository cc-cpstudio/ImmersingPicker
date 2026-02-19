package com.github.immersingeducation.immersingpicker.config

import com.github.immersingeducation.immersingpicker.config.enums.SelectedAmountWeightCalculateMode
import kotlinx.coroutines.*

object ConfigUtils {
    val defConfig = mutableMapOf(
        "privateCG" to ConfigGroup("privateCG", mapOf(
            "isFirstRun" to ConfigItem(
                name = "isFirstRun",
                desc = "isFirstRun",
                value = true
            ),
            "currentClass" to ConfigItem(
                name = "currentClass",
                desc = "currentClass",
                value = null
            )
        )),
        "selectorCG" to ConfigGroup("抽选器设置", mapOf(
            "fairSelection" to ConfigItem(
                name = "开启公平抽选？",
                desc = "开启后将会多维度使每位学生的抽选次数均匀分配",
                value = true
            ),
            "thresholdValueOfSelectionPool" to ConfigItem(
                name = "候选池学生数量阈值",
                desc = "候选池里学生的最小数量",
                value = 1
            ),
            "selectedAmountWeightCalcMode" to ConfigItem(
                name = "抽选次数权重计算方式",
                desc = "更改方式后，每位学生的权重差和相对顺序可能会发生变化",
                value = SelectedAmountWeightCalculateMode.EXPONENT
            ),
            "selectedAmountWeightCalcCoefficient" to ConfigItem(
                name = "抽选次数权重计算系数",
                desc = "更改系数后，每位学生的权重差可能会发生变化，并产生一些奇妙效果",
                value = 1.12
            )
        ))
    )

    val config = mutableMapOf<String, ConfigGroup>()

    val listeners = mutableMapOf<ConfigItem, MutableList<ConfigChangeListener>>()

    fun loadExistingConfig() {
        TODO("等待数据持久化")
    }

    fun getConfig(id: String): ConfigItem? {
        config.forEach { (_, group) ->
            group.configs.forEach { (nameOfItem, item) ->
                if (nameOfItem == id) {
                    return item
                }
            }
        }
        return null
    }

    fun setConfig(name: String, value: Any?) {
        var flag = false
        config.forEach { (_, group) ->
            group.configs.forEach { (nameOfItem, item) ->
                if (nameOfItem == name) {
                    flag = true
                    item.value = value
                    if (item.needRestart) {
                        TODO("需要重启")
                    }
                }
            }
        }
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