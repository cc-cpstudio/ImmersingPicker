package com.github.immersingeducation.immersingpicker.config

import com.github.immersingeducation.immersingpicker.config.enums.SelectedAmountWeightCalculateMode
import com.github.immersingeducation.immersingpicker.tools.convertToGenericType

object ConfigUtils {
    val defConfig = mutableMapOf(
        "selectorCG" to ConfigGroup("抽选器设置", mapOf(
            "fairSelection" to ConfigItem<Boolean>(
                name = "开启公平抽选？",
                desc = "开启后将会多维度使每位学生的抽选次数均匀分配",
                def = true,
                value = null
            ),
            "thresholdValueOfSelectionPool" to ConfigItem<Int>(
                name = "候选池学生数量阈值",
                desc = "候选池里学生的最小数量",
                def = 1,
                value = null
            ),
            "selectedAmountWeightCalcMode" to ConfigItem<SelectedAmountWeightCalculateMode>(
                name = "抽选次数权重计算方式",
                desc = "更改方式后，每位学生的权重差和相对顺序可能会发生变化",
                def = SelectedAmountWeightCalculateMode.EXPONENT,
                value = null
            ),
            "selectedAmountWeightCalcCoefficient" to ConfigItem<Double>(
                name = "抽选次数权重计算系数",
                desc = "更改系数后，每位学生的权重差可能会发生变化，并产生一些奇妙效果",
                def = 1.12,
                value = null
            )
        ))
    )

    val config = mutableMapOf<String, ConfigGroup>()

    fun loadExistingConfig() {

    }

    fun generateDefaultConfig() {
    }

    fun getConfig(name: String): ConfigItem<out Any>? {
        config.forEach { (nameOfGroup, group) ->
            group.configs.forEach { (nameOfItem, item) ->
                if (nameOfItem == name) {
                    return item
                }
            }
        }
        return null
    }

    fun setConfig(name: String, value: Any?) {
        config.forEach { (nameOfGroup, group) ->
            group.configs.forEach { (nameOfItem, item) ->
                if (nameOfItem == name) {
                    @Suppress("UNCHECKED_CAST")
                    (item as ConfigItem<Any>).value = convertToGenericType(item.def, value)
                }
            }
        }
    }
}