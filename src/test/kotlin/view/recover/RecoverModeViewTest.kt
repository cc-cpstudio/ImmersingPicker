package view.recover

import com.github.immersingeducation.immersingpicker.view.recover.RecoverModeView
import tornadofx.*

class TestApp: App(RecoverModeView::class)

fun main() {
    launch<TestApp>()
}