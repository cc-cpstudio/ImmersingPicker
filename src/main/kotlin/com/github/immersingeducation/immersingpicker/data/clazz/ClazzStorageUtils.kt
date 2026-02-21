package com.github.immersingeducation.immersingpicker.data.clazz

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.History
import com.github.immersingeducation.immersingpicker.tools.BasicUtils
import mu.KotlinLogging
import org.yaml.snakeyaml.DumperOptions
import org.yaml.snakeyaml.Yaml
import java.io.FileInputStream
import java.io.FileWriter
import java.time.LocalDateTime

object ClazzStorageUtils {
    val logger = KotlinLogging.logger {}

    fun saveClasses() {
        val dumperOptions = DumperOptions().apply {
            defaultFlowStyle = DumperOptions.FlowStyle.BLOCK
        }
        val yaml = Yaml(dumperOptions)
        logger.trace("成功创建Yaml解析器对象")
        
        // 使用映射表方式构建数据结构
        val classesMap = mapOf(
            "classes" to Clazz.classes.map { clazz ->
                mapOf(
                    "name" to clazz.name,
                    "students" to clazz.students.map { student ->
                        mapOf(
                            "name" to student.name,
                            "id" to student.id,
                            "seatRow" to student.seatRow,
                            "seatColumn" to student.seatColumn,
                            "initialWeight" to student.initialWeight,
                            "lastSelectedTime" to student.lastSelectedTime,
                            "selectedAmount" to student.selectedAmount,
                            "weight" to student.weight
                        )
                    },
                    "historyList" to clazz.historyList
                )
            },
            "current" to Clazz.currentIndex
        )
        
        val yamlString = yaml.dump(classesMap)
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

    fun loadClasses() {
        val yaml = Yaml()
        logger.trace("成功创建Yaml解析器对象")
        try {
            FileInputStream("${BasicUtils.getWorkDirPath()}/ipicker/classes.yml").use { reader ->
                val map = yaml.load(reader) as Map<String, Any>
                val classesList = map["classes"] as List<Map<String, Any>>
                val currentIndex = map["current"] as? Int?
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
                val classes = Classes(storableClasses, currentIndex)
                val loadedClasses = TransitionUtils.classesToList(classes)
                
                // 清空现有班级列表并添加加载的班级
                Clazz.classes.clear()
                Clazz.classes.addAll(loadedClasses)
                Clazz.currentIndex = currentIndex
                logger.info("成功从文件加载Yaml字符串，并存储到Clazz.classes和Clazz.current中")
            }
        } catch (e: Exception) {
            logger.error("加载Clazz列表时发生异常", e)
        }
    }
}