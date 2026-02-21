package com.github.immersingeducation.immersingpicker.data.clazz

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.History
import com.github.immersingeducation.immersingpicker.tools.BasicUtils
import mu.KotlinLogging
import org.yaml.snakeyaml.DumperOptions
import org.yaml.snakeyaml.LoaderOptions
import org.yaml.snakeyaml.Yaml
import org.yaml.snakeyaml.constructor.Constructor
import org.yaml.snakeyaml.nodes.Tag
import org.yaml.snakeyaml.representer.Representer
import java.io.File
import java.io.FileInputStream
import java.io.FileWriter
import java.time.LocalDateTime

object ClazzStorageUtils {
    val logger = KotlinLogging.logger {}

    fun saveClasses() {
        val representer = Representer(DumperOptions().apply {
            defaultFlowStyle = DumperOptions.FlowStyle.BLOCK
        })
        representer.addClassTag(Classes::class.java, Tag.MAP)
        val yaml = Yaml(representer)
        logger.trace("成功创建Yaml解析器对象")
        val classes = TransitionUtils.listToClasses(Clazz.classes)
        val yamlString = yaml.dump(classes)
        logger.debug("成功将Clazz列表转换为Yaml字符串")
        try {
            FileWriter("${BasicUtils.getWorkDirPath()}/ipicker/classes.yml").use { writer ->
                writer.write(yamlString)
                logger.debug("成功将Yaml字符串写入文件")
            }
            logger.info("成功保存Clazz列表到文件：${BasicUtils.getWorkDirPath()}/ipicker/classes.yml")
        } catch (e: Exception) {
            logger.error("保存Clazz列表到文件时发生异常", e)
        }
    }

    fun loadClasses(): List<Clazz> {
        val yaml = Yaml()
        logger.trace("成功创建Yaml解析器对象")
        try {
            FileInputStream("${BasicUtils.getWorkDirPath()}/ipicker/classes.yml").use { reader ->
                val map = yaml.load(reader) as Map<String, Any>
                val classesList = map["classes"] as List<Map<String, Any>>
                val storableClasses = classesList.map {
                    val name = it["name"] as String
                    val students = (it["students"] as List<Map<String, Any>>).map { student ->
                        val studentName = student["name"] as String
                        val id = student["id"] as Int
                        val seatRow = student["seatRow"] as Int
                        val seatColumn = student["seatColumn"] as Int
                        val initialWeight = student["initialWeight"] as Double
                        val lastSelectedTime = student["lastSelectedTime"] as? LocalDateTime?
                        val selectedAmount = student["selectedAmount"] as Int
                        val weight = student["weight"] as Double
                        StorableStudent(studentName, id, seatRow, seatColumn, initialWeight, lastSelectedTime, selectedAmount, weight)
                    }.toMutableList()
                    val historyList = emptyList<History>()
                    StorableClazz(name, students, historyList)
                }
                val classes = Classes(storableClasses)
                logger.info("成功从文件加载Yaml字符串，并转为Clazz列表")
                return TransitionUtils.classesToList(classes)
            }
        } catch (e: Exception) {
            logger.error("加载Clazz列表时发生异常，将返回空列表", e)
            return emptyList()
        }
    }
}