# Python Learning Game Demo

This project is a prototype for an educational game that teaches middle school students the basics of Python programming. The project includes **two main components**:

1. **Parsons Problems**  
   - A drag-and-drop interface where students reorder lines of Python code to form a correct program.  
   - Provides instant AI-powered feedback on whether the solution is correct.  
   - Encourages logical thinking and understanding of Python syntax without enforcing a single coding style.

2. **Input-Based Exercises**  
   - A windowed interface where students can write Python code freely in a text box.  
   - Supports multi-line input, including variable assignments and function definitions.  
   - Code is executed in a restricted namespace, and the system checks behavior/output rather than the exact syntax.  
   - AI-powered feedback guides students on what they did correctly, what can be improved, and next steps.  
   - Students progress through a sequence of exercises, building on previous code.

---

## Features

- **AI-Powered Feedback:** Uses Google’s Gemini 2.0 model to provide short, friendly, and age-appropriate guidance for each exercise.  
- **Windowed GUI:** Students do not need to interact with the terminal.  
- **Multiple exercises:** Sequential progression from simple variable assignments to basic function creation.  
- **Lenient evaluation:** Students can be creative, as long as the code behaves as expected.  
- **Cumulative context:** Previous exercises carry over, so students can reference earlier code.  
- **Clean, polished interface:** Large fonts, bordered code input, highlighted buttons, and clear feedback areas.

---

## Requirements

- Python 3.10 +  
   - [Windows](https://www.python.org/downloads/windows)
   - [MacOS](https://www.python.org/downloads/macos)
- Internet connection for AI feedback (access to Google Gemini 2.0 API)
- Run the following commands in a PowerShell terminal with Python installed:
```bash
pip install google-genai

pip install PyQt5
```
*These allow for the AI to function properly, and for the Graphical User Interface (GUI) to work.*