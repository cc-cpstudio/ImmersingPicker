package com.github.immersingeducation.immersingpicker.fonts

import javafx.scene.text.Font
import java.io.FileInputStream

object FontLoaderFX {
    fun loadFontFromResources(path: String, defaultSize: Double): Font? {
        try {
            FontLoaderFX.javaClass.getResourceAsStream(path).use { stream ->
                return if (stream != null) { Font.loadFont(stream, defaultSize) } else { null }
            }
        } catch (e: Exception) {
            return null
        }
    }

    fun loadFontFromFile(path: String, defaultSize: Double): Font? {
        return try {
            Font.loadFont(FileInputStream(path), defaultSize)
        } catch (e: Exception) {
            null
        }
    }
}