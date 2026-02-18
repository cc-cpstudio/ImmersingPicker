package view.main

import com.github.immersingeducation.immersingpicker.view.main.MainView
import tornadofx.*

class TestApp: App(MainView::class)

fun main() {
    launch<TestApp>()
}