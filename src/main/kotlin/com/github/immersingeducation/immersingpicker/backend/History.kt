package com.github.immersingeducation.immersingpicker.backend

import mu.KotlinLogging
import java.time.LocalDateTime

data class History(
    val createTime: LocalDateTime = LocalDateTime.now(),
    val selector: String,
    val students: List<Student>
) {
    companion object {
        val logger = KotlinLogging.logger {}
    }
}