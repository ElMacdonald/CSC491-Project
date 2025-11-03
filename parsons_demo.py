import sys
import random
from PyQt5.QtWidgets import (
    QApplication, QWidget, QVBoxLayout, QListWidget, QListWidgetItem,
    QLabel, QPushButton, QMessageBox
)
from PyQt5.QtCore import Qt
from PyQt5.QtGui import QFont, QKeySequence
from PyQt5.QtWidgets import QShortcut
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

# --- Parsons Problems ---
PROBLEMS = [
    {
        "description": "Define a function called square that returns the square of a number n.",
        "solution": [
            "def square(n):",
            "    result = n * n",
            "    return result"
        ]
    },
    {
        "description": "Define a function called greet that prints 'Hello!'.",
        "solution": [
            "def greet():",
            "    print(\"Hello!\")"
        ]
    },
    {
        "description": "Define a function called max_of_two that returns the larger of two numbers a and b.",
        "solution": [
            "def max_of_two(a, b):",
            "    if a > b:",
            "        return a",
            "    else:",
            "        return b"
        ]
    }
]

# --- Helper to shuffle lines ---
def shuffled_lines(solution_lines):
    shuffled = solution_lines.copy()
    while True:
        random.shuffle(shuffled)
        if shuffled != solution_lines:
            return shuffled

# --- AI feedback ---
def get_ai_feedback(problem_desc, student_code_lines, correct=False):
    try:
        student_code = "\n".join(student_code_lines)
        if correct:
            return f"Correct! Well done! Your code works because it matches the solution for: {problem_desc}"

        prompt = (
            f"Exercise description:\n{problem_desc}\n\n"
            f"Student code:\n{student_code}\n\n"
            "Give very short, simple, friendly feedback for a student aged 8-12. " \
            "Use easy words and short sentences. " \
            "Explain what lines are correct, what can be improved, and what to try next. " \
            "Keep it concise, school-appropriate, and do not use long paragraphs or big words. " \
            "Do not give the correct answer. " \
            "If the answer is correct, congratulate the student and explain why it is correct. " \
            "Treat the student code as rearranged lines in a Parsons problem. " \
            "Only provide the feedback text, no extra commentary."

        )
        response = CLIENT.models.generate_content(
            model="gemini-2.0-flash",
            contents=prompt
        )
        return response.text
    except Exception as e:
        return f"(AI Feedback unavailable: {e})"

# --- Main Window ---
class ParsonsWindow(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Parsons Problem Demo")
        self.resize(1200, 1000)
        self.problem_index = 0

        self.layout = QVBoxLayout()
        self.setLayout(self.layout)

        # Problem description
        self.description_label = QLabel()
        self.description_label.setWordWrap(True)
        self.description_label.setFont(QFont("Arial", 16))
        self.layout.addWidget(self.description_label)

        # Instruction label (static)
        self.instruction_label = QLabel("Drag and drop the lines into the correct order.")
        self.instruction_label.setFont(QFont("Arial", 14))
        self.layout.addWidget(self.instruction_label)

        # AI feedback label
        self.ai_feedback_label = QLabel()
        self.ai_feedback_label.setFont(QFont("Arial", 14))
        self.ai_feedback_label.setStyleSheet("color: #333333;")
        self.ai_feedback_label.setWordWrap(True)
        self.layout.addWidget(self.ai_feedback_label)

        # Draggable list widget
        self.list_widget = QListWidget()
        self.list_widget.setFont(QFont("Consolas", 14))
        self.list_widget.setDragDropMode(QListWidget.InternalMove)
        self.list_widget.setStyleSheet("""
            QListWidget::item {
                border: 2px solid #555555;
                padding: 8px;
                margin: 2px;
                border-radius: 5px;
                background-color: #f0f0f0;
            }
            QListWidget::item:selected {
                background-color: #a0c4ff;
            }
        """)
        self.layout.addWidget(self.list_widget)

        # Check answer button
        self.check_button = QPushButton("Check Answer")
        self.check_button.setFont(QFont("Arial", 14))
        self.check_button.clicked.connect(self.check_answer)
        self.layout.addWidget(self.check_button)

        # Shortcut: Shift + Enter triggers check_answer
        shortcut = QShortcut(QKeySequence("Shift+Return"), self)
        shortcut.activated.connect(self.check_answer)

        # Load first problem
        self.load_problem()

    def load_problem(self):
        self.list_widget.clear()
        if self.problem_index < len(PROBLEMS):
            problem = PROBLEMS[self.problem_index]
            problem_number = self.problem_index + 1
            self.description_label.setText(f"<b>Problem {problem_number}: {problem['description']}</b>")
            self.ai_feedback_label.setText("")  # Clear previous AI feedback
            self.current_lines = shuffled_lines(problem["solution"])
            for line in self.current_lines:
                item = QListWidgetItem(line)
                item.setTextAlignment(Qt.AlignLeft)
                self.list_widget.addItem(item)
        else:
            self.description_label.setText("You have completed all problems!")
            self.list_widget.setDisabled(True)
            self.check_button.setDisabled(True)

    def check_answer(self):
        if self.problem_index >= len(PROBLEMS):
            return

        current_order = [self.list_widget.item(i).text() for i in range(self.list_widget.count())]
        solution = PROBLEMS[self.problem_index]["solution"]

        correct = current_order == solution
        ai_feedback = get_ai_feedback(PROBLEMS[self.problem_index]["description"], current_order, correct)
        self.ai_feedback_label.setText(ai_feedback)

        if correct:
            QMessageBox.information(
                self,
                "Correct",
                f"Correct! You have completed Problem {self.problem_index + 1}."
            )
            self.problem_index += 1
            if self.problem_index < len(PROBLEMS):
                self.load_problem()
            else:
                self.close()

# --- Run Application ---
if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = ParsonsWindow()
    window.show()
    sys.exit(app.exec_())
