package component

import com.github.immersingeducation.immersingpicker.component.Seat
import com.github.immersingeducation.immersingpicker.component.SeatGrid
import com.github.immersingeducation.immersingpicker.core.ClassNGrade
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
    override val root = vbox(spacing = 5.0) {
        paddingAll = 5.0

//        add(Seat(
//            Student(
//                id = 1,
//                name = "Alice",
//                seatRow = 1,
//                seatColumn = 1
//            )
//        ))
//
//        add(Seat(
//            Student(
//                id = 2,
//                name = "Bob",
//                seatRow = 1,
//                seatColumn = 2
//            )
//        ))

        add(SeatGrid(
            ClassNGrade(
                name = "name",
                students = mutableListOf(
                    Student(
                        id = 1,
                        name = "Alice",
                        seatRow = 1,
                        seatColumn = 1
                    ),
                    Student(
                        id = 2,
                        name = "Bob",
                        seatRow = 1,
                        seatColumn = 2
                    ),
                    Student(
                        id = 3,
                        name = "Charlie",
                        seatRow = 2,
                        seatColumn = 1
                    ),
                    Student(
                        id = 4,
                        name = "David",
                        seatRow = 2,
                        seatColumn = 2
                    )
                ),
                historyList = mutableListOf()
            )
        ))
    }
}

fun main() {
    launch<TestApp>()
}