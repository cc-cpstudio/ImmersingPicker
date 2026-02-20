package com.github.immersingeducation.immersingpicker.data.clazz

import com.github.immersingeducation.immersingpicker.core.Clazz
import com.github.immersingeducation.immersingpicker.tools.BasicUtils
import org.yaml.snakeyaml.DumperOptions
import org.yaml.snakeyaml.LoaderOptions
import org.yaml.snakeyaml.Yaml
import org.yaml.snakeyaml.constructor.Constructor
import org.yaml.snakeyaml.nodes.Tag
import org.yaml.snakeyaml.representer.Representer
import java.io.FileWriter

object ClazzStorageUtils {
    val yaml = Yaml()

    fun saveClasses() {
        val classes = TransitionUtils.listToClasses(Clazz.classes)
        val yamlString = yaml.dump(classes)
        FileWriter("${BasicUtils.getWorkDirPath()}/classes.yml").use { writer ->
            writer.write(yamlString)
        }
    }
}
