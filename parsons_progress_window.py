import sys
from PyQt5.QtWidgets import QApplication, QWidget, QVBoxLayout, QLabel, QTableWidget, QTableWidgetItem, QPushButton
from PyQt5.QtGui import QFont
from database_manager import load_progress_data

PROBLEMS = [
    {"description": "Define function square(n)", "solution": ["def square(n):", "    return n*n"]},
    {"description": "Define function greet()", "solution": ["def greet():", "    print('Hello!')"]},
]

class ParsonsProgressWindow(QWidget):
    def __init__(self, username="guest"):
        super().__init__()
        self.username = username
        self.setWindowTitle("Parsons Progress")
        self.resize(850, 500)
        layout = QVBoxLayout(self)
        
        title = QLabel(f"Progress for {self.username}")
        title.setFont(QFont("Arial", 18, QFont.Bold))
        layout.addWidget(title)

        self.table = QTableWidget()
        self.table.setColumnCount(4)
        self.table.setHorizontalHeaderLabels(["Question", "Selected Answer(s)", "Correct Answer(s)", "Result"])
        layout.addWidget(self.table)

        self.summary_label = QLabel()
        layout.addWidget(self.summary_label)

        close_button = QPushButton("Close")
        close_button.clicked.connect(self.close)
        layout.addWidget(close_button)

        self.load_progress()

    def load_progress(self):
        progress_data = load_progress_data(self.username, "parsons")
        self.table.setRowCount(len(PROBLEMS))

        completed = 0
        correct_count = 0

        # Turn DB rows into a dictionary for quick lookup
        progress_dict = {}
        for entry in progress_data:
            # Structure: (username, lesson_type, problem_index, is_correct, selected_answer)
            if len(entry) >= 5:
                progress_dict[entry[2]] = {
                    "is_correct": bool(entry[3]),
                    "selected": entry[4] or ""
                }

        for i, problem in enumerate(PROBLEMS):
            desc = problem["description"]
            correct_answer = "\n".join(problem["solution"])
            selected_answer = ""
            result = ""

            if i in progress_dict:
                completed += 1
                selected_answer = progress_dict[i]["selected"]
                if progress_dict[i]["is_correct"]:
                    result = "✅ Correct"
                    correct_count += 1
                else:
                    result = "❌ Incorrect"
            else:
                # Hide answers if not completed
                correct_answer = ""
                selected_answer = ""
                result = ""

            self.table.setItem(i, 0, QTableWidgetItem(desc))
            self.table.setItem(i, 1, QTableWidgetItem(selected_answer))
            self.table.setItem(i, 2, QTableWidgetItem(correct_answer))
            self.table.setItem(i, 3, QTableWidgetItem(result))

        self.summary_label.setText(
            f"Total: {len(PROBLEMS)} | Completed: {completed} | Correct: {correct_count} | Incorrect: {completed - correct_count}"
        )

if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = ParsonsProgressWindow("student1")
    window.show()
    sys.exit(app.exec_())
