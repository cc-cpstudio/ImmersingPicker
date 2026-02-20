package data

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.data.DataPersistence
import org.junit.jupiter.api.AfterEach
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import java.io.File

class DataPersistenceTest {
    @BeforeEach
    fun setUp() {
        // 清空Clazz.classes列表
        Clazz.classes.clear()
        
        // 创建测试班级
        val class1 = Clazz("class1")
        class1.addStudent("学生1", 1, Pair(1, 1))
        class1.addStudent("学生2", 2, Pair(1, 2))
        
        val class2 = Clazz("class2")
        class2.addStudent("学生3", 3, Pair(2, 1))
        class2.addStudent("学生4", 4, Pair(2, 2))
    }
    
    @Test
    fun testSaveClasses() {
        // 调用saveClasses方法
        DataPersistence.saveClasses()
        
        // 验证文件是否存在
        val classesFile = File("ipicker/classes.yml")
        assert(classesFile.exists()) {
            "classes.yml文件不存在"
        }
        
        // 验证文件内容是否不为空
        assert(classesFile.length() > 0) {
            "classes.yml文件内容为空"
        }
    }
    
    @AfterEach
    fun tearDown() {
        // 清空Clazz.classes列表
        Clazz.classes.clear()
        
        // 删除测试生成的文件和目录
        val classesFile = File("ipicker/classes.yml")
        if (classesFile.exists()) {
            classesFile.delete()
        }
        
        val ipickerDir = File("ipicker")
        if (ipickerDir.exists()) {
            ipickerDir.delete()
        }
    }
}
