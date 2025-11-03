import sys
from PyQt5.QtWidgets import (
    QApplication, QWidget, QVBoxLayout, QLabel, QPushButton, QMessageBox, QTextEdit
)
from PyQt5.QtGui import QFont
from PyQt5.QtCore import Qt
from google import genai
import os

# --- Load API key from file ---
KEY_FILE = "api_key.txt"  # Make sure this is in .gitignore

def load_api_key():
    if os.path.exists(KEY_FILE):
        with open(KEY_FILE, "r") as f:
            return f.read().strip()
    return None

API_KEY = load_api_key()
if not API_KEY:
    raise ValueError(f"API key not found in {KEY_FILE}")

# --- Initialize AI client ---
CLIENT = genai.Client(api_key=API_KEY)

# --- Subclass QTextEdit to handle Shift+Enter and smaller tab width ---
class CodeTextEdit(QTextEdit):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.submit_callback = None
        # Make tabs visually 4 spaces
        self.setTabStopDistance(self.fontMetrics().horizontalAdvance('  ') * 4)

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
        "description": "Create a variable 'x' and assign it the value 10.",
        "var_name": "x",
        "expected_value": 10
    },
    {
        "type": "variable",
        "description": "Create a variable 'y' and assign it the value of x + 5.",
        "var_name": "y",
        "expected_value": 15
    },
    {
        "type": "function",
        "description": "Define a function 'add' that takes two numbers and returns their sum.",
        "func_name": "add",
        "test_cases": [((2,3),5), ((-1,5),4), ((0,0),0)]
    }
]

# --- AI feedback ---
def get_ai_feedback(exercise_desc, student_code):
    try:
        prompt = (
            f"Exercise description:\n{exercise_desc}\n\n"
            f"Student code:\n{student_code}\n\n"
            "Give very short, simple, friendly feedback for a student aged 8-12. "
            "Use easy words, short sentences. Explain what is correct, what can be improved, and what to try next. "
            "Keep it concise, school-appropriate, and do not use long paragraphs or big words. "
            "If the answer is correct, congratulate the student and explain why. "
            "Only provide the feedback text. Do not restate the question or give the correct answer directly. "
            "The code is from a parsons-style exercise; treat it as rearranged code lines."
        )
        response = CLIENT.models.generate_content(
            model="gemini-2.0-flash",
            contents=prompt
        )
        return response.text
    except Exception as e:
        return f"(AI Feedback unavailable: {e})"

# --- PyQt5 GUI ---
class InputLessonWindow(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Python Input-Based Lesson")
        self.resize(1000, 800)

        self.layout = QVBoxLayout()
        self.layout.setSpacing(15)
        self.setLayout(self.layout)

        # Exercise description
        self.description_label = QLabel("")
        self.description_label.setWordWrap(True)
        self.description_label.setFont(QFont("Arial", 18, QFont.Bold))
        self.layout.addWidget(self.description_label)

        # AI feedback label
        self.ai_feedback_label = QLabel("")
        self.ai_feedback_label.setWordWrap(True)
        self.ai_feedback_label.setFont(QFont("Arial", 14))
        self.ai_feedback_label.setStyleSheet("color: #333333;")
        self.layout.addWidget(self.ai_feedback_label)

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
        self.submit_button = QPushButton("Run and Check")
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
            self.description_label.setText(f"<b>Exercise {self.exercise_index + 1}: {ex['description']}</b>")
            self.ai_feedback_label.setText("")
            self.code_input.clear()
        else:
            QMessageBox.information(self, "Amazing!", "You have completed all exercises.")
            self.close()

    def check_code(self):
        student_code = self.code_input.toPlainText()
        full_code = self.previous_code + "\n" + student_code
        ex = EXERCISES[self.exercise_index]

        # Evaluate correctness
        correct = False
        try:
            safe_locals = {}
            exec(full_code, {}, safe_locals)
            if ex["type"] == "variable":
                correct = (safe_locals.get(ex["var_name"], None) == ex["expected_value"])
            elif ex["type"] == "function":
                func = safe_locals.get(ex["func_name"], None)
                if callable(func):
                    correct = all(func(*args) == expected for args, expected in ex["test_cases"])
        except Exception:
            correct = False

        # AI feedback only
        ai_feedback = get_ai_feedback(ex['description'], full_code)
        self.ai_feedback_label.setText(f"AI Feedback:\n{ai_feedback}")

        if correct:
            QMessageBox.information(
                self,
                "Correct",
                f"Exercise {self.exercise_index + 1} completed correctly!"
            )
            self.previous_code = full_code
            self.exercise_index += 1
            self.load_exercise()

# --- Run Application ---
if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = InputLessonWindow()
    window.show()
    sys.exit(app.exec_())
