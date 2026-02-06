package com.github.immersingeducation.immersingpicker.backend.core

import java.time.LocalDateTime

data class History(
    val createTime: LocalDateTime = LocalDateTime.now(),
    val selector: String,
    val students: List<Student>
)