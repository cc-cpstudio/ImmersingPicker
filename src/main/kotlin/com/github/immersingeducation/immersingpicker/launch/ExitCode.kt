package com.github.immersingeducation.immersingpicker.launch

class ExitCode {
    companion object {
        const val MANUAL_EXIT = 0
        const val MANUAL_RESTART = 1
        const val UNKNOWN = -1

        const val EXCEPTION_EXIT = 400
        const val EXCEPTION_RESTART = 401
    }
}
