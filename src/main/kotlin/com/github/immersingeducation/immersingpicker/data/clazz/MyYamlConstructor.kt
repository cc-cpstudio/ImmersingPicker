package com.github.immersingeducation.immersingpicker.data.clazz

import org.yaml.snakeyaml.constructor.Constructor
import org.yaml.snakeyaml.LoaderOptions
import org.yaml.snakeyaml.nodes.Node
import org.yaml.snakeyaml.nodes.ScalarNode
import org.yaml.snakeyaml.nodes.Tag
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import java.time.format.DateTimeParseException

// 自定义构造器，适配 SnakeYAML 2.x
class MyYamlConstructor(targetType: Class<*>, loaderOptions: LoaderOptions) : Constructor(targetType, loaderOptions) {
    private val dateTimeFormatters = listOf(
        DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")
    )

    override fun getConstructor(node: Node): org.yaml.snakeyaml.constructor.Construct? {
        if (node.tag == Tag.STR && node is ScalarNode) {
            val value = node.value
            if (isLocalDateTimeString(value)) {
                return ConstructLocalDateTime()
            }
        }
        return super.getConstructor(node)
    }

    inner class ConstructLocalDateTime : org.yaml.snakeyaml.constructor.Construct {
        override fun construct(node: Node): Any? {
            val scalarNode = node as ScalarNode
            val timeStr = scalarNode.value
            dateTimeFormatters.forEach { formatter ->
                try {
                    return LocalDateTime.parse(timeStr, formatter)
                } catch (e: DateTimeParseException) {
                }
            }
            throw IllegalArgumentException("无法解析时间字符串：$timeStr，支持格式：$dateTimeFormatters")
        }
        override fun construct2ndStep(node: Node, data: Any?) {}
    }

    private fun isLocalDateTimeString(str: String): Boolean {
        dateTimeFormatters.forEach { formatter ->
            try {
                LocalDateTime.parse(str, formatter)
                return true
            } catch (e: DateTimeParseException) {

            }
        }
        return false
    }
}