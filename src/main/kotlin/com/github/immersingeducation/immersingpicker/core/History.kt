package com.github.immersingeducation.immersingpicker.core

import java.time.LocalDateTime

/**
 * 历史记录类，用于表示班级的历史记录
 * @param createTime 历史记录创建时间
 * @param selector 这次记录是用哪个抽选器创建的
 * @param students 历史记录中的学生列表
 * @author CC想当百大
 * @since v1.0.0.a
 */
data class History(
    val createTime: LocalDateTime = LocalDateTime.now(),
    val selector: String,
    val students: List<Student>
)