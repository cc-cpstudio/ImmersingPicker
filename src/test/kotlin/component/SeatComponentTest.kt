package component

import com.github.immersingeducation.immersingpicker.component.Seat
import com.github.immersingeducation.immersingpicker.core.Student
import javafx.stage.Stage
import tornadofx.*

class TestApp: App(TestView::class) {
    override fun start(stage: Stage) {
        super.start(stage)
        stage.apply {
            minWidth = 400.0
            minHeight = 300.0
        }
    }
}

class TestView: View() {
    override val root = vbox {
        add(Seat(
            Student(
                id = 1,
                name = "Alice",
                initialWeight = 1.0,
                seatRow = 1,
                seatColumn = 1
            )
        ))
    }
}

fun main() {
    launch<TestApp>()
}