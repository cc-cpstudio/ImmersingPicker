package com.github.immersingeducation.immersingpicker.config.enums

/**
 * 计算权重过程中，代入抽选次数计算时的计算方式
 * @author CC想当百大
 * @since v1.0.0.a
 */
enum class SelectedAmountWeightCalculateMode(string: String) {
    LINEAR("线性"),
    POWER("乘方"),
    EXPONENT("指数"),
    LOG_E("自然对数"),
    LOG_10("常用对数")
}