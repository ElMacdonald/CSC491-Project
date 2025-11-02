import sys
from PyQt5.QtWidgets import (
    QApplication, QWidget, QVBoxLayout, QLabel, QPushButton, QMessageBox
)
from PyQt5.QtGui import QFont
from PyQt5.QtCore import Qt
from PyQt5.QtWidgets import QTextEdit

# --- Subclass QTextEdit to handle Shift+Enter ---
class CodeTextEdit(QTextEdit):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.submit_callback = None  # Function to call on Shift + Enter

    def keyPressEvent(self, event):
        if event.key() == Qt.Key_Return and event.modifiers() & Qt.ShiftModifier:
            if self.submit_callback:
                self.submit_callback()
        else:
            super().keyPressEvent(event)

# --- Example exercises ---
EXERCISES = [
    {
        "type": "variable",
        "description": "Exercise 1: Create a variable 'x' and assign it the value 10.",
        "var_name": "x",
        "expected_value": 10
    },
    {
        "type": "variable",
        "description": "Exercise 2: Create a variable 'y' and assign it the value of x + 5.",
        "var_name": "y",
        "expected_value": 15
    },
    {
        "type": "function",
        "description": "Exercise 3: Define a function 'add' that takes two numbers and returns their sum.",
        "func_name": "add",
        "test_cases": [((2,3),5), ((-1,5),4), ((0,0),0)]
    }
]

# --- Helper functions ---
def check_variable(student_code, var_name, expected_value):
    safe_locals = {}
    try:
        exec(student_code, {}, safe_locals)
    except Exception as e:
        return False, f"Error in your code: {e}"

    if var_name not in safe_locals:
        return False, f"Variable '{var_name}' is not defined."
    
    student_value = safe_locals[var_name]
    if student_value == expected_value:
        return True, "Correct!"
    else:
        return False, f"Variable '{var_name}' is defined, but its value is {student_value}. Expected {expected_value}."

def check_function(student_code, func_name, test_cases):
    safe_locals = {}
    try:
        exec(student_code, {}, safe_locals)
    except Exception as e:
        return False, f"Error in your code: {e}"

    if func_name not in safe_locals or not callable(safe_locals[func_name]):
        return False, f"Function '{func_name}' is not defined correctly."

    func = safe_locals[func_name]
    success = True
    feedback_messages = []

    for args, expected in test_cases:
        try:
            result = func(*args)
            if result != expected:
                success = False
                feedback_messages.append(
                    f"For input {args}, expected {expected}, got {result}."
                )
        except Exception as e:
            success = False
            feedback_messages.append(f"For input {args}, error occurred: {e}")

    if success:
        return True, "Correct!"
    else:
        return False, " | ".join(feedback_messages)

# --- PyQt5 GUI ---
class InputLessonWindow(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Python Input-Based Lesson")
        self.resize(700, 600)

        self.layout = QVBoxLayout()
        self.layout.setSpacing(15)
        self.setLayout(self.layout)

        # Exercise description
        self.description_label = QLabel("")
        self.description_label.setWordWrap(True)
        self.description_label.setFont(QFont("Arial", 18, QFont.Bold))
        self.layout.addWidget(self.description_label)

        # Feedback label
        self.feedback_label = QLabel("")
        self.feedback_label.setWordWrap(True)
        self.feedback_label.setFont(QFont("Arial", 14))
        self.feedback_label.setStyleSheet("color: #333333;")
        self.layout.addWidget(self.feedback_label)

        # Code input box
        self.code_input = CodeTextEdit()
        self.code_input.setFont(QFont("Consolas", 14))
        self.code_input.setStyleSheet("""
            QTextEdit {
                border: 2px solid #555555;
                border-radius: 5px;
                padding: 8px;
                background-color: #f8f8f8;
            }
        """)
        self.code_input.setPlaceholderText("Type your Python code here...")
        self.layout.addWidget(self.code_input)

        # Connect Shift+Enter to check_code
        self.code_input.submit_callback = self.check_code

        # Submit button
        self.submit_button = QPushButton("Run & Check")
        self.submit_button.setFont(QFont("Arial", 16, QFont.Bold))
        self.submit_button.setStyleSheet("""
            QPushButton {
                background-color: #4CAF50;
                color: white;
                padding: 12px;
                border-radius: 8px;
            }
            QPushButton:hover {
                background-color: #45a049;
            }
        """)
        self.submit_button.clicked.connect(self.check_code)
        self.layout.addWidget(self.submit_button)

        # Lesson tracking
        self.exercise_index = 0
        self.previous_code = ""
        self.load_exercise()

    def load_exercise(self):
        if self.exercise_index < len(EXERCISES):
            ex = EXERCISES[self.exercise_index]
            self.description_label.setText(ex["description"])
            self.feedback_label.setText("Write your code in the box below and click 'Run & Check' or press Shift + Enter.")
            self.code_input.clear()
        else:
            QMessageBox.information(self, "Lesson Complete", "You have completed all exercises.")
            self.close()

    def check_code(self):
        student_code = self.code_input.toPlainText()
        full_code = self.previous_code + "\n" + student_code
        ex = EXERCISES[self.exercise_index]

        if ex["type"] == "variable":
            correct, feedback = check_variable(full_code, ex["var_name"], ex["expected_value"])
        elif ex["type"] == "function":
            correct, feedback = check_function(full_code, ex["func_name"], ex["test_cases"])
        else:
            correct, feedback = False, "Unknown exercise type."

        self.feedback_label.setText(feedback)
        if correct:
            self.previous_code = full_code
            self.exercise_index += 1
            self.load_exercise()

# --- Run Application ---
if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = InputLessonWindow()
    window.show()
    sys.exit(app.exec_())
