import sys
import random
import os
from PyQt5.QtWidgets import (
    QApplication, QWidget, QVBoxLayout, QListWidget, QListWidgetItem,
    QLabel, QPushButton, QMessageBox, QShortcut
)
from PyQt5.QtCore import Qt
from PyQt5.QtGui import QFont, QKeySequence
import google.generativeai as genai

# --- Load API key from file ---
SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
KEY_FILE = os.path.join(SCRIPT_DIR, "api_key.txt")

def load_api_key():
    if os.path.exists(KEY_FILE):
        with open(KEY_FILE, "r") as f:
            return f.read().strip()
    return None


API_KEY = load_api_key()
if not API_KEY:
    raise ValueError(f"API key not found in {KEY_FILE}")

# --- Initialize AI client (FIXED) ---
genai.configure(api_key=API_KEY)
MODEL = genai.GenerativeModel("gemini-2.0-flash")

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

# --- AI feedback (FIXED) ---
def get_ai_feedback(problem_desc, student_code_lines, correct=False):
    try:
        student_code = "\n".join(student_code_lines)

        if correct:
            return f"Correct! Well done! Your code works because it matches the solution for: {problem_desc}"

        prompt = (
            f"Exercise description:\n{problem_desc}\n\n"
            f"Student code:\n{student_code}\n\n"
            "Give very short, simple, friendly feedback for a student aged 8-12. "
            "Use easy words and short sentences. "
            "Explain what lines are correct, what can be improved, and what to try next. "
            "Keep it concise, school-appropriate, and do not use long paragraphs or big words. "
            "Do not give the correct answer. "
            "If the answer is correct, congratulate the student and explain why it is correct. "
            "Treat the student code as rearranged lines in a Parsons problem. "
            "Only provide the feedback text, no extra commentary."
        )

        response = MODEL.generate_content(prompt)
        return response.text

    except Exception as e:
        return f"(AI Feedback unavailable: {e})"


# --- Progress tracking ---
PROGRESS_FILE = "parsons_progress.txt"

def load_progress():
    progress = {}
    last_unfinished = 0
    if os.path.exists(PROGRESS_FILE):
        with open(PROGRESS_FILE, "r") as f:
            lines = f.read().splitlines()

        current_q = None
        for line in lines:
            line = line.strip()
            if line.endswith(":") and line[:-1].isdigit():
                current_q = int(line[:-1])
                progress[current_q] = {"attempts": [], "completed": False}
            elif line.startswith("Attempt") and current_q is not None:
                attempt_text = line.split(":", 1)[1].strip()
                progress[current_q]["attempts"].append(attempt_text)
            elif line.lower() == "incomplete" and current_q is not None:
                progress[current_q]["completed"] = False

        # Find first unfinished question
        for q in range(1, len(PROBLEMS)+1):
            if q not in progress or not progress[q]["completed"]:
                last_unfinished = q - 1 if q > 1 else 0
                break
        else:
            last_unfinished = len(PROBLEMS)

    return progress, last_unfinished

def save_progress(progress):
    with open(PROGRESS_FILE, "w") as f:
        for q in range(1, len(PROBLEMS) + 1):
            f.write(f"{q}:\n")
            if q in progress:
                attempts = progress[q]["attempts"]
                if attempts:
                    for i, att in enumerate(attempts, start=1):
                        f.write(f"Attempt {i}:\n")
                        for line in att.split("\n"):
                            f.write(f"{line}\n")

                        feedback_key = f"{q}_attempt_{i}_feedback"
                        if feedback_key in progress[q]:
                            f.write(f"\nAI Feedback:\n{progress[q][feedback_key]}\n")
                        f.write("\n")

                if not progress[q].get("completed", False) and not attempts:
                    f.write("Incomplete\n")
            else:
                f.write("Incomplete\n")

            f.write("\n")


# --- GUI Window ---
class ParsonsWindow(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Parsons Problem Demo")
        self.resize(1200, 1000)

        # Load progress
        self.progress, last_unfinished = load_progress()
        self.problem_index = last_unfinished

        self.layout = QVBoxLayout()
        self.setLayout(self.layout)

        self.description_label = QLabel()
        self.description_label.setWordWrap(True)
        self.description_label.setFont(QFont("Arial", 16))
        self.layout.addWidget(self.description_label)

        self.instruction_label = QLabel("Drag and drop the lines into the correct order.")
        self.instruction_label.setFont(QFont("Arial", 14))
        self.layout.addWidget(self.instruction_label)

        self.ai_feedback_label = QLabel()
        self.ai_feedback_label.setFont(QFont("Arial", 14))
        self.ai_feedback_label.setWordWrap(True)
        self.ai_feedback_label.setStyleSheet("color: #333333;")
        self.layout.addWidget(self.ai_feedback_label)

        self.list_widget = QListWidget()
        self.list_widget.setFont(QFont("Consolas", 14))
        self.list_widget.setDragDropMode(QListWidget.InternalMove)
        self.layout.addWidget(self.list_widget)

        self.check_button = QPushButton("Check Answer")
        self.check_button.setFont(QFont("Arial", 14))
        self.check_button.clicked.connect(self.check_answer)
        self.layout.addWidget(self.check_button)

        shortcut = QShortcut(QKeySequence("Shift+Return"), self)
        shortcut.activated.connect(self.check_answer)

        self.load_problem()

    def load_problem(self):
        self.list_widget.clear()
        if self.problem_index < len(PROBLEMS):
            problem = PROBLEMS[self.problem_index]
            num = self.problem_index + 1
            self.description_label.setText(f"<b>Problem {num}: {problem['description']}</b>")
            self.ai_feedback_label.setText("")
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

        q_num = self.problem_index + 1

        if q_num not in self.progress:
            self.progress[q_num] = {"attempts": [], "completed": False}

        self.progress[q_num]["attempts"].append("\n".join(current_order))

        attempt_index = len(self.progress[q_num]["attempts"])
        self.progress[q_num][f"{q_num}_attempt_{attempt_index}_feedback"] = ai_feedback

        if correct:
            self.progress[q_num]["completed"] = True

        save_progress(self.progress)

        if correct:
            QMessageBox.information(
                self,
                "Correct!",
                f"You finished Problem {self.problem_index + 1}."
            )
            self.problem_index += 1
            if self.problem_index < len(PROBLEMS):
                self.load_problem()
            else:
                self.close()


# --- Run ---
if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = ParsonsWindow()
    window.show()
    sys.exit(app.exec_())
