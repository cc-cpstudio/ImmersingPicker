package com.github.immersingeducation.immersingpicker.tools

inline fun <reified T> convertToGenericType(ref: T, value: Any?): T? {
    return value as? T
}