import sys
import random
from PyQt5.QtWidgets import (
    QApplication, QWidget, QVBoxLayout, QListWidget, QListWidgetItem,
    QLabel, QPushButton, QMessageBox
)
from PyQt5.QtCore import Qt
from PyQt5.QtGui import QFont

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

# --- Helper to shuffle lines but never present already solved ---
def shuffled_lines(solution_lines):
    shuffled = solution_lines.copy()
    while True:
        random.shuffle(shuffled)
        if shuffled != solution_lines:
            return shuffled

# --- Main Window ---
class ParsonsWindow(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Parsons Problem Demo")
        self.resize(700, 600)  # Bigger window
        self.problem_index = 0

        self.layout = QVBoxLayout()
        self.setLayout(self.layout)

        # Problem description
        self.description_label = QLabel()
        self.description_label.setWordWrap(True)
        self.description_label.setFont(QFont("Arial", 16))
        self.layout.addWidget(self.description_label)

        # Feedback label
        self.feedback_label = QLabel()
        self.feedback_label.setFont(QFont("Arial", 14))
        self.layout.addWidget(self.feedback_label)

        # Draggable list widget
        self.list_widget = QListWidget()
        self.list_widget.setFont(QFont("Consolas", 14))
        self.list_widget.setDragDropMode(QListWidget.InternalMove)

        # Style list items: border, padding, rounded corners, highlight
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

        # Load first problem
        self.load_problem()

    def load_problem(self):
        self.list_widget.clear()
        problem = PROBLEMS[self.problem_index]
        self.description_label.setText(f"<b>{problem['description']}</b>")
        self.feedback_label.setText("Drag and drop the lines into the correct order.")

        self.current_lines = shuffled_lines(problem["solution"])
        for line in self.current_lines:
            item = QListWidgetItem(line)
            item.setTextAlignment(Qt.AlignLeft)
            self.list_widget.addItem(item)

    def check_answer(self):
        current_order = [self.list_widget.item(i).text() for i in range(self.list_widget.count())]
        solution = PROBLEMS[self.problem_index]["solution"]
        if current_order == solution:
            QMessageBox.information(self, "Correct!", "✅ Correct! Great job!")
            self.problem_index += 1
            if self.problem_index < len(PROBLEMS):
                self.load_problem()
            else:
                QMessageBox.information(self, "Finished", "🎉 You've completed all problems!")
                self.close()
        else:
            self.feedback_label.setText("❌ Not quite right. Keep trying!")

# --- Run Application ---
if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = ParsonsWindow()
    window.show()
    sys.exit(app.exec_())
