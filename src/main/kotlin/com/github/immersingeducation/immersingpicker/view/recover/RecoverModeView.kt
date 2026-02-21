package com.github.immersingeducation.immersingpicker.view.recover

import com.github.immersingeducation.immersingpicker.config.ConfigUtils
import com.github.immersingeducation.immersingpicker.style.RecoverViewStyleUtils
import javafx.geometry.Pos
import javafx.scene.text.FontWeight
import tornadofx.*

class RecoverModeView: View() {
    init {
        importStylesheet(RecoverViewStyleUtils::class)
    }

    override val root = vbox {
        alignment = Pos.CENTER_LEFT
        paddingAll = 10.0
        spacing = 10.0

        style {
            minWidth = 300.px
            maxWidth = 300.px
        }

        label("恢复模式") {
            style {
                fontSize = 20.px
                fontWeight = FontWeight.EXTRA_BOLD
            }
        }

        label("请选择下列选项以继续。") {
            style {
                fontSize = 12.px
                fontWeight = FontWeight.LIGHT
            }
        }

        button("切换当前班级") {
            addClass(RecoverViewStyleUtils.normalButton)

            action {
                ClassChangeDialog()
                    .showAndWait()
                    ?.ifPresent { result ->
                        ConfigUtils.setConfig("currentClass", result)
                    }
            }
        }

        button("继续使用 ImmersingPicker") {
            addClass(RecoverViewStyleUtils.normalButton)
        }

        button("以调试模式使用 ImmersingPicker") {
            addClass(RecoverViewStyleUtils.normalButton)
        }

        button("打开日志目录") {
            addClass(RecoverViewStyleUtils.normalButton)
        }

        button("备份班级数据") {
            addClass(RecoverViewStyleUtils.normalButton)
        }

        button("备份配置") {
            addClass(RecoverViewStyleUtils.normalButton)
        }

        button("恢复班级数据") {
            addClass(RecoverViewStyleUtils.normalButton)
        }

        button("恢复配置") {
            addClass(RecoverViewStyleUtils.normalButton)
        }

        button("重置配置") {
            addClass(RecoverViewStyleUtils.dangerousButton)
        }

        button("重置全部数据") {
            addClass(RecoverViewStyleUtils.dangerousButton)
        }
    }
}