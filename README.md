# Python Learning Game Demo

This project is a prototype for an educational game that teaches middle school students the basics of Python programming. The project includes **two main components**:

1. **Parsons Problems**  
   - A drag-and-drop interface where students reorder lines of Python code to form a correct program.  
   - Provides instant feedback on whether the solution is correct.  
   - Encourages logical thinking and understanding of Python syntax without enforcing a single coding style.

2. **Input-Based Exercises**  
   - A windowed interface where students can write Python code freely in a text box.  
   - Supports multi-line input, including variable assignments and function definitions.  
   - Code is executed in a restricted namespace, and the system checks behavior/output rather than the exact syntax.  
   - Feedback is provided for each exercise, and students progress through a sequence of exercises.

---

## Features

- **Windowed GUI:** Students do not need to interact with the terminal.
- **Multiple exercises:** Sequential progression from simple variable assignments to basic function creation.
- **Lenient evaluation:** Students can be creative, as long as the code behaves as expected.
- **Cumulative context:** Previous exercises carry over, so students can reference earlier code.
- **Clean, polished interface:** Large fonts, bordered code input, highlighted buttons, and clear feedback areas.

---

## Requirements

- Python 3.10+  
- [PyQt5](https://pypi.org/project/PyQt5/)

Install PyQt5 using pip:

```bash
pip install PyQt5
