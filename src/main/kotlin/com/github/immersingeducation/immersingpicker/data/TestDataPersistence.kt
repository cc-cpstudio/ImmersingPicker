package com.github.immersingeducation.immersingpicker.data

import com.github.immersingeducation.immersingpicker.core.Clazz

fun main(args: Array<String>) {
    // 清空Clazz.classes列表
    Clazz.classes.clear()
    
    // 创建测试班级
    val class1 = Clazz("class1")
    class1.addStudent("学生1", 1, Pair(1, 1))
    class1.addStudent("学生2", 2, Pair(1, 2))
    
    val class2 = Clazz("class2")
    class2.addStudent("学生3", 3, Pair(2, 1))
    class2.addStudent("学生4", 4, Pair(2, 2))
    
    // 调用saveClasses方法
    DataPersistence.saveClasses()
    
    println("数据持久化测试完成！")
    println("请检查ipicker/classes.yml文件是否生成成功。")
}
