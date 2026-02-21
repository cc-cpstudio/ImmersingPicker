package data

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.Student
import com.github.immersingeducation.immersingpicker.data.clazz.PeriodicalClazzStorageUtils
import kotlinx.coroutines.delay
import kotlinx.coroutines.runBlocking

fun main() {
    // 首先创建一个测试班级和学生，以便测试保存功能
    val clazz = Clazz(
        name = "testClass",
        students = mutableListOf(
            Student(
                name = "testStudent1",
                id = 1,
                seatRow = 1,
                seatColumn = 1
            ),
            Student(
                name = "testStudent2",
                id = 2,
                seatRow = 1,
                seatColumn = 2
            )
        ),
        historyList = mutableListOf()
    )
    
    println("测试开始：PeriodicalClazzStorageUtils")
    
    // 测试启动定期保存任务
    println("1. 启动定期保存任务")
    PeriodicalClazzStorageUtils.start()
    println("   任务已启动")
    
    // 等待一段时间，让任务执行几次
    runBlocking {
        println("2. 等待3秒，让任务执行几次")
        delay(3000)
    }
    
    // 测试停止定期保存任务
    println("3. 停止定期保存任务")
    PeriodicalClazzStorageUtils.stop()
    println("   任务已停止")
    
    // 再次启动任务，测试重复启动的情况
    println("4. 再次启动定期保存任务")
    PeriodicalClazzStorageUtils.start()
    println("   任务已再次启动")
    
    // 测试任务已经运行时再开启任务的情况
    println("5. 测试任务已经运行时再开启任务的情况")
    PeriodicalClazzStorageUtils.start()
    println("   任务已经在运行，尝试再次启动完成")
    
    // 等待一段时间
    runBlocking {
        println("6. 等待2秒")
        delay(2000)
    }
    
    // 最终停止任务
    println("7. 最终停止定期保存任务")
    PeriodicalClazzStorageUtils.stop()
    println("   任务已最终停止")
    
    println("\n测试完成：PeriodicalClazzStorageUtils")
    println("请查看日志输出，确认任务是否正常执行和停止")
}