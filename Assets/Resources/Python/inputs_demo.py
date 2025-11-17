import os
import google.generativeai as genai

# ---- FILE PATHS ----
BASE_DIR = os.path.dirname(os.path.abspath(__file__))

API_KEY_FILE = os.path.join(BASE_DIR, "api_key.txt")
PLAYER_INPUT_FILE = os.path.join(BASE_DIR, "player_input.txt")
SAMPLE_SOLUTION_FILE = os.path.join(BASE_DIR, "sample_solution.txt")
OUTPUT_FILE = os.path.join(BASE_DIR, "ai_feedback.txt")


# ---- LOAD API KEY (same logic you used originally) ----
def load_api_key():
    if not os.path.exists(API_KEY_FILE):
        raise ValueError(f"API key not found: {API_KEY_FILE}")

    with open(API_KEY_FILE, "r", encoding="utf-8") as f:
        return f.read().strip()


API_KEY = load_api_key()
print("key loaded")

# ---- INITIALIZE AI (exactly like your working code) ----
genai.configure(api_key=API_KEY)
MODEL = genai.GenerativeModel("gemini-2.0-flash")


# ---- FILE UTILITIES ----
def read_file(path):
    if not os.path.exists(path):
        return "(file missing)"
    with open(path, "r", encoding="utf-8") as f:
        return f.read()


def write_file(path, text):
    with open(path, "w", encoding="utf-8") as f:
        f.write(text)


# ---- PROMPT ----
def build_prompt(player_input, sample_solution):
    return f"""
You are an AI tutor for a young beginner (age 8–12).

The game provides two text files:

FILE #1 — Player Input:
(Problem + student's code)
=====================
{player_input}
=====================

FILE #2 — Sample Solution:
(Problem + correct code)
=====================
{sample_solution}
=====================

TASK:
You MUST follow these rules exactly:

1. Output ONLY these three sections, in this exact order:
   Problem:
   Your Code:
   Feedback:

2. Include NOTHING before, after, or between those sections.
   - No greetings
   - No introductions
   - No “Okay!” or “Let's see”
   - No emojis
   - No extra commentary

3. Do not output the sample solution code.

4. Compare the student's code to the problem and the sample solution.

5. Decide if the student solved the problem correctly.

6. If correct:
   - Praise them briefly.
   - Explain why in simple words.

7. If incorrect:
   - Give **short, simple hints**.
   - Explain every incorrect piece, don't leave anything out.
   - Suggest what to try next.
   - Do NOT reveal the correct answer.

8. Keep the feedback VERY short, friendly, and kid-safe.

9. Keep the preamble to a minimum.

10. Do NOT use the sample solution code in your response.

You must ONLY produce the required three sections and nothing else.
"""


# ---- MAIN EVALUATION ----
def run_evaluator():
    print("Reading input files...")

    player_input = read_file(PLAYER_INPUT_FILE)
    sample_solution = read_file(SAMPLE_SOLUTION_FILE)

    prompt = build_prompt(player_input, sample_solution)

    print("Calling Gemini model...")

    try:
        response = MODEL.generate_content(prompt)
        feedback = response.text.strip()
    except Exception as e:
        feedback = f"(AI Error: {e})"

    print("Writing ai_feedback.txt...")

    # ---- DELETE old file if it exists ----
    if os.path.exists(OUTPUT_FILE):
        os.remove(OUTPUT_FILE)

    # ---- Write fresh file ----
    write_file(OUTPUT_FILE, feedback)

    print("Done.")


if __name__ == "__main__":
    run_evaluator()
    print("Evaluator run complete.")
