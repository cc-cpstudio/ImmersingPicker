module com.github.immersingeducation.immersingpicker {
    requires javafx.controls;
    requires javafx.fxml;
    requires kotlin.stdlib;

    requires org.controlsfx.controls;
    requires org.kordamp.ikonli.javafx;
    requires org.kordamp.bootstrapfx.core;

    requires tornadofx;

    requires io.github.microutils.kotlinlogging;
    requires org.slf4j;

    opens com.github.immersingeducation.immersingpicker to javafx.fxml;
    exports com.github.immersingeducation.immersingpicker;
}