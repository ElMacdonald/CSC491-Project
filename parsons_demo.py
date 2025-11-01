import random

# A list of Parsons problems.
# Each problem is a dict with a description and the correct solution lines.
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
    }
]

def present_problem(problem):
    print("\n--- PARSONS PROBLEM ---")
    print("Task:", problem["description"], "\n")

    # Copy and shuffle solution lines
    shuffled = problem["solution"].copy()
    random.shuffle(shuffled)

    print("Arrange the following lines in the correct order:")
    for i, line in enumerate(shuffled):
        print(f"{i+1}: {line}")

    # Get user input
    user_input = input("\nEnter the correct order as space-separated numbers (e.g., 2 1 3): ")
    try:
        order = list(map(int, user_input.split()))
    except ValueError:
        print("Invalid input. Please enter numbers only.")
        return False
    
    # Build the user's attempt based on their chosen order
    attempt = [shuffled[i-1] for i in order]

    # Check correctness
    if attempt == problem["solution"]:
        print("\n✅ Correct! Great job!")
        return True
    else:
        print("\n❌ That's not quite right.")
        print("Here is your attempt:")
        for line in attempt:
            print(line)
        print("\nTry again or review the logic.")
        return False


def run_lesson():
    print("Welcome! Let's solve a few programming structure puzzles.\n")

    for problem in PROBLEMS:
        solved = False
        while not solved:
            solved = present_problem(problem)

    print("\n🎉 You've completed all current problems! Well done.")


if __name__ == "__main__":
    run_lesson()
