package com.github.immersingeducation.immersingpicker.selectors

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.core.History
import com.github.immersingeducation.immersingpicker.core.MAX_AVAIL_RANGE
import com.github.immersingeducation.immersingpicker.core.MIN_SELECTION_POOL_AMOUNT
import com.github.immersingeducation.immersingpicker.core.Student
import mu.KotlinLogging
import java.time.LocalDateTime
import java.time.temporal.ChronoUnit
import java.util.Random
import kotlin.math.pow

/**
 * 选择器抽象基类，用于定义选择器的基本行为和属性
 * @param clazz 班级对象，用于操作班级中的学生和历史记录
 * @author CC想当百大
 * @since v1.0.0.a
 */
abstract class SelectorBase(val clazz: Clazz) {
    companion object{
        val random = Random()
        val logger = KotlinLogging.logger {}
    }

    abstract val name: String

    /**
     * 选择逻辑，根据班级中的学生和历史记录选择指定数量的学生
     * @param amount 要选择的学生数量
     * @return 历史记录对象，包含选择的学生和时间
     * @author CC想当百大
     * @since v1.0.0.a
     */
    abstract fun selectLogic(amount: Int): History

    /**
     * 计算极差，用于确定选择学生的范围
     * @param needed 班级中需要选择的学生列表
     * @return 极差，即选择学生的范围
     * @author CC想当百大
     * @since v1.0.0.a
     */
    private fun calculateRange(needed: List<Student>): Int {
        val maxE = needed.maxBy { it.selectedAmount }
        val minE = needed.minBy { it.selectedAmount }
        logger.trace("计算极差：max=${maxE.selectedAmount}, min=${minE.selectedAmount}")
        return maxE.selectedAmount - minE.selectedAmount
    }

    /**
     * 计算可用选择学生列表，根据班级中的学生和历史记录筛选出可选择的学生
     * @return 可用选择学生列表
     * @author CC想当百大
     * @since v1.0.0.a
     */
    private fun calculateAvailableSelectStudents(): MutableList<Student> {
        logger.debug("开始计算可用选择学生")
        val tmpStudents = clazz.students
        logger.trace("初始学生数量: ${tmpStudents.size}")
        var availableRange = calculateRange(tmpStudents)
        logger.trace("初始可用范围: $availableRange")
        while (availableRange > MAX_AVAIL_RANGE && tmpStudents.size >= MIN_SELECTION_POOL_AMOUNT) {
            val maxSelectedStudent = clazz.students.maxBy { it.selectedAmount }
            logger.trace("移除选择次数最多的学生: ${maxSelectedStudent.name} (${maxSelectedStudent.selectedAmount}次)")
            tmpStudents.remove(maxSelectedStudent)
            availableRange = calculateRange(tmpStudents)
            logger.trace("更新后可用范围: $availableRange")
        }
        val average = clazz.students.sumOf { it.selectedAmount } / clazz.students.size
        logger.trace("班级学生平均选择次数: $average")
        tmpStudents.forEach {
            if (it.selectedAmount >= average) {
                logger.trace("移除选择次数大于等于平均的学生: ${it.name} (${it.selectedAmount}次)")
                clazz.students.remove(it)
            }
        }
        logger.debug("计算完成，可用选择学生数量: ${tmpStudents.size}")
        return tmpStudents
    }

    /**
     * 计算学生权重，根据班级中的学生和历史记录计算每个学生的权重
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun calculateWeight() {
        logger.debug("开始计算学生权重")
        // 权重影响因素：抽取次数，上次抽取时间与现在的间隔，
        val available = calculateAvailableSelectStudents()
        val average = available.sumOf { it.selectedAmount } / available.size
        logger.trace("可用学生平均选择次数: $average")
        
        clazz.students.forEach {
            if (it.weight <= 1.0) {
                logger.trace("重置学生 ${it.name} 的权重为 1.0")
                it.weight  = 1.0
            }
        }
        
        clazz.students.forEach {
            logger.trace("计算学生 ${it.name} 的权重")
            if (available.contains(it)) {
                val weightIncrease = (average - it.selectedAmount) * 1.7
                logger.trace("学生 ${it.name} 在可用列表中，权重增加: $weightIncrease")
                it.weight += weightIncrease
            } else {
                logger.trace("学生 ${it.name} 不在可用列表中，权重增加: 0.8")
                it.weight += 0.8
            }
            
            if (it.lastSelectedTime != null && ChronoUnit.DAYS.between(LocalDateTime.now(), it.lastSelectedTime) > 3) {
                val daysBetween = ChronoUnit.DAYS.between(it.lastSelectedTime, LocalDateTime.now())
                val timeWeight = 1.12.pow(daysBetween.toDouble())
                logger.trace("学生 ${it.name} 上次选择时间间隔超过3天，权重增加: $timeWeight")
                it.weight += timeWeight
            } else {
                logger.trace("学生 ${it.name} 上次选择时间间隔未超过3天，权重增加: 0.0")
                it.weight += ChronoUnit.DAYS.between(LocalDateTime.now(), LocalDateTime.now()).toDouble() * 1.01
            }
            
            val randomWeight = random.nextDouble(1.0, 5.0)
            logger.trace("学生 ${it.name} 随机权重增加: $randomWeight")
            it.weight += randomWeight
            
            logger.trace("学生 ${it.name} 最终权重: ${it.weight}")
        }
        
        logger.debug("权重计算完成")
    }

    /**
     * 选择学生，根据班级中的学生和历史记录选择指定数量的学生
     * @param amount 需要选择的学生数量
     * @return 选择的学生历史记录
     * @author CC想当百大
     * @since v1.0.0.a
     */
    fun select(amount: Int): History {
        logger.debug("开始选择学生，需要选择 $amount 人")
        try {
            calculateWeight()
            logger.trace("权重计算完成，开始执行选择逻辑")
            val history = selectLogic(amount)
            logger.trace("选择逻辑执行完成，选择了 ${history.students.size} 人")
            
            history.students.forEach {
                logger.trace("更新学生 ${it.name} 的选择信息")
                it.lastSelectedTime = history.createTime
                it.selectedAmount ++
                logger.trace("学生 ${it.name} 已被选择 ${it.selectedAmount} 次")
            }
            
            logger.info("选择完成，选择了 ${history.students.size} 人：${history.students.joinToString { it.name }}")
            return history
        } catch (e: Exception) {
            logger.error("选择学生时发生异常", e)
            throw e
        }
    }
}