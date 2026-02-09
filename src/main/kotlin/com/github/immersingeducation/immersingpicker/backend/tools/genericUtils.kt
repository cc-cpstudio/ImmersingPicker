package com.github.immersingeducation.immersingpicker.backend.tools

inline fun <reified T> convertToGenericType(ref: T, value: Any?): T? {
    return value as? T
}