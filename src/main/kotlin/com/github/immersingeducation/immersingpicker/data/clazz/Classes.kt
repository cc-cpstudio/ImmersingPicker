package com.github.immersingeducation.immersingpicker.data.clazz

/**
 * 班级列表类，用于表示班级列表和当前操作的班级索引
 * @param classes 班级列表
 * @param currentIndex 当前班级索引，没有则为 null
 * @author CC想当百大
 * @since v1.0.0.a
 */
data class Classes(
    val classes: List<StorableClazz>,
    val currentIndex: Int?
)