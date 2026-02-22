package com.github.immersingeducation.immersingpicker.config

/**
 * 配置ChangeListener接口，用于监听配置项的变化
 * @author CC想当百大
 * @since v1.0.0.a
 */
fun interface ConfigChangeListener {
    /**
     * 当配置项发生变化时调用
     * @param old 配置项变化前的旧值
     * @param new 配置项变化后的新值
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun onConfigChanged(old: Any?, new: Any?)
}