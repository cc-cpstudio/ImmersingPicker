package com.github.immersingeducation.immersingpicker.backend.config.enums

enum class SelectedAmountWeightCalculateMode(string: String) {
    LINEAR("线性"),
    POWER("乘方"),
    EXPONENT("指数"),
    LOG_E("自然对数"),
    LOG_10("常用对数")
}