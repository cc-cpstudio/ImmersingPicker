package com.github.immersingeducation.immersingpicker.data

import com.github.immersingeducation.immersingpicker.core.Clazz
import mu.KotlinLogging
import org.yaml.snakeyaml.DumperOptions
import org.yaml.snakeyaml.Yaml
import java.io.File
import java.io.FileWriter

object DataPersistence {
    private val logger = KotlinLogging.logger {}
    private const val IPICKER_DIR = "ipicker"
    private const val CLASSES_FILE = "classes.yml"
    
    fun saveClasses() {
        try {
            // 创建ipicker目录
            val ipickerDir = File(IPICKER_DIR)
            if (!ipickerDir.exists()) {
                if (ipickerDir.mkdir()) {
                    logger.info("成功创建ipicker目录")
                } else {
                    logger.error("创建ipicker目录失败")
                    return
                }
            }
            
            // 转换所有班级为StorableClazz
            val storableClasses = Clazz.classes.map { StorableClazz.serialize(it) }
            
            // 创建包含所有班级的映射
            val data = mapOf("classes" to storableClasses)
            
            // 配置snakeyaml
            val options = DumperOptions()
            options.setIndent(2)
            options.setPrettyFlow(true)
            options.setDefaultFlowStyle(DumperOptions.FlowStyle.BLOCK)
            
            // 创建Yaml实例
            val yaml = Yaml(options)
            
            // 写入文件
            val classesFile = File(ipickerDir, CLASSES_FILE)
            FileWriter(classesFile).use {
                yaml.dump(data, it)
            }
            
            logger.info("成功保存班级数据到${classesFile.absolutePath}")
        } catch (e: Exception) {
            logger.error("保存班级数据失败: ${e.message}")
            e.printStackTrace()
        }
    }
}
