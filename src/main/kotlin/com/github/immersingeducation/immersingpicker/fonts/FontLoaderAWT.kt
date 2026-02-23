package com.github.immersingeducation.immersingpicker.fonts

import java.awt.Font
import java.awt.FontFormatException
import java.awt.GraphicsEnvironment
import java.io.File
import java.io.FileNotFoundException
import java.io.InputStream
import java.net.URL

object FontLoaderAWT {
    fun loadFontFromResources(path: String, fontSize: Float, fontStyle: Int): Font? {
        return if (path.isEmpty()) {
            null
        } else {
            val fontURL: URL = javaClass.getResource(path) ?: return null
            try {
                fontURL.openStream().use { stream: InputStream ->
                    val baseFont = Font.createFont(Font.TRUETYPE_FONT, stream)
                    GraphicsEnvironment.getLocalGraphicsEnvironment().registerFont(baseFont)
                    baseFont.deriveFont(fontStyle, fontSize)
                }
            } catch (e: FontFormatException) {
                null
            } catch (e: Exception) {
                null
            }
        }
    }

    fun loadFontFromFile(path: String, fontSize: Float, fontStyle: Int): Font? {
        return if (path.isEmpty()) {
            null
        } else {
            val fontFile = File(path)
            if (!fontFile.exists() || !fontFile.isFile) {
                null
            } else {
                try {
                    val baseFont = Font.createFont(Font.TRUETYPE_FONT, fontFile)
                    GraphicsEnvironment.getLocalGraphicsEnvironment().registerFont(baseFont)
                    baseFont.deriveFont(fontStyle, fontSize)
                } catch (e: FontFormatException) {
                    null
                } catch (e: FileNotFoundException) {
                    null
                } catch (e: Exception) {
                    null
                }
            }
        }
    }
}