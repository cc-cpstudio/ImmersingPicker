package com.github.immersingeducation.immersingpicker.data.clazz

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.History

/**
 * 可存储的班级类，用于表示班级列表和当前操作的班级索引
 * @param name 班级名称
 * @param students 班级中的学生列表
 * @param historyList 班级的历史记录列表
 * @author CC想当百大
 * @since v1.0.0.a
 */
data class StorableClazz(
    var name: String,
    var students: MutableList<StorableStudent>,
    var historyList: List<History>
)