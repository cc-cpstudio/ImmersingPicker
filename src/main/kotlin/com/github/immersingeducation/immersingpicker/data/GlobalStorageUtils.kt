package com.github.immersingeducation.immersingpicker.data

import com.github.immersingeducation.immersingpicker.data.clazz.ClazzStorageUtils
import com.github.immersingeducation.immersingpicker.data.config.ConfigStorageUtils
import kotlinx.coroutines.CancellationException
import kotlinx.coroutines.CoroutineDispatcher
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.SupervisorJob
import kotlinx.coroutines.cancel
import kotlinx.coroutines.delay
import kotlinx.coroutines.isActive
import kotlinx.coroutines.launch
import mu.KotlinLogging
import java.util.concurrent.atomic.AtomicBoolean
import kotlin.time.Duration.Companion.milliseconds

class GlobalStorageUtils private constructor(
    dispatcher: CoroutineDispatcher = Dispatchers.IO
) {
    private val coroutineScope = CoroutineScope(SupervisorJob() + dispatcher)
    private val hasActiveJob = AtomicBoolean(false)
    private var currentJob: Job? = null

    fun saveClassesPeriodically() {
        cancelCurrent()
        hasActiveJob.set(true)
        currentJob = coroutineScope.launch {
            try {
                while (isActive) {
                    try {
                        logger.debug("开始尝试保存班级数据")
                        ClazzStorageUtils.saveClasses()
                        logger.debug("班级数据保存完成")
                    } catch (e: Exception) {
                        logger.error("本次保存班级数据时出错", e)
                    }

                    try {
                        logger.debug("开始尝试保存配置数据")
                        ConfigStorageUtils.saveConfig()
                        logger.debug("配置数据保存完成")
                    } catch (e: Exception) {
                        logger.error("本次保存配置数据时出错", e)
                    }

                    try {
                        logger.debug("开始尝试保存默认配置数据")
                        ConfigStorageUtils.saveDefaultConfig()
                        logger.debug("默认配置数据保存完成")
                    } catch (e: Exception) {
                        logger.error("本次保存默认配置数据时出错", e)
                    }

                    logger.debug("全部执行完毕，开始等待")
                    delay(1000.milliseconds)
                }
            } catch (e: CancellationException) {
                logger.info("保存数据的周期任务被取消")
            } finally {
                hasActiveJob.set(false)
                logger.debug("周期任务结束")
            }
        }
    }

    fun cancelCurrent() {
        if (currentJob?.isActive == true) {
            currentJob?.cancel()
        }
        currentJob = null
    }

    fun destroy() {
        cancelCurrent()
        coroutineScope.cancel()
        logger.debug("周期任务已被销毁")
    }

    companion object {
        val logger = KotlinLogging.logger {}

        private var utilsObject: GlobalStorageUtils? = null
        private val isTaskRunning = AtomicBoolean(false)

        fun start() {
            if (!isTaskRunning.get()) {
                utilsObject = GlobalStorageUtils()
                utilsObject?.saveClassesPeriodically()
                logger.debug("周期任务已启动")
                isTaskRunning.set(true)
            }
        }

        fun stop() {
            if (isTaskRunning.get()) {
                utilsObject?.cancelCurrent()
                utilsObject?.destroy()
                utilsObject = null
                logger.debug("周期任务已停止")
                isTaskRunning.set(false)
            }
        }

        fun loadData() {
            logger.info("进入 loadData 函数")
            ClazzStorageUtils.loadClasses()
            ConfigStorageUtils.loadDefaultConfig()
            ConfigStorageUtils.loadConfig()
            logger.info("成功结束 loadData 函数")
        }
    }
}