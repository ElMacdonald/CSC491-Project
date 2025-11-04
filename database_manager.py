import sqlite3

DB_NAME = "progress.db"

def get_connection():
    return sqlite3.connect(DB_NAME)

def initialize_database():
    conn = get_connection()
    c = conn.cursor()
    c.execute("""
        CREATE TABLE IF NOT EXISTS progress (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            username TEXT NOT NULL,
            lesson_type TEXT NOT NULL,
            problem_index INTEGER NOT NULL,
            is_correct INTEGER DEFAULT 0,
            selected_answer TEXT,
            attempts INTEGER DEFAULT 0
        )
    """)
    conn.commit()
    conn.close()

def get_existing_entry(username, lesson_type, problem_index):
    conn = get_connection()
    c = conn.cursor()
    c.execute("""
        SELECT id, attempts FROM progress
        WHERE username=? AND lesson_type=? AND problem_index=?
    """, (username, lesson_type, problem_index))
    row = c.fetchone()
    conn.close()
    return row

def save_progress(username, lesson_type, problem_index, is_correct, selected_answer):
    conn = get_connection()
    c = conn.cursor()
    existing = get_existing_entry(username, lesson_type, problem_index)
    if existing:
        id_, attempts = existing
        c.execute("""
            UPDATE progress
            SET is_correct=?, selected_answer=?, attempts=?
            WHERE id=?
        """, (int(is_correct), selected_answer, attempts + 1, id_))
    else:
        c.execute("""
            INSERT INTO progress (username, lesson_type, problem_index, is_correct, selected_answer, attempts)
            VALUES (?, ?, ?, ?, ?, 1)
        """, (username, lesson_type, problem_index, int(is_correct), selected_answer))
    conn.commit()
    conn.close()

def load_progress_data(username, lesson_type):
    conn = get_connection()
    c = conn.cursor()
    c.execute("""
        SELECT problem_index, is_correct, selected_answer, attempts
        FROM progress
        WHERE username=? AND lesson_type=?
    """, (username, lesson_type))
    rows = c.fetchall()
    conn.close()
    return rows

def export_progress_to_file(username, lesson_type, problems):
    data = load_progress_data(username, lesson_type)
    progress_map = {r[0]: r for r in data}
    with open("progress_report.txt", "w", encoding="utf-8") as f:
        f.write(f"Progress Report for {username} ({lesson_type})\n")
        f.write("=" * 60 + "\n\n")
        for i, p in enumerate(problems):
            f.write(f"Q{i+1}: {p['description']}\n")
            if i in progress_map:
                _, correct, selected, attempts = progress_map[i]
                f.write(f"  Attempts: {attempts}\n")
                f.write(f"  Result: {'✅ Correct' if correct else '❌ Incorrect'}\n")
                f.write(f"  Selected Answer:\n{selected}\n")
                f.write(f"  Correct Answer:\n{chr(10).join(p['solution'])}\n\n")
            else:
                f.write("  [Not Attempted Yet]\n\n")
    print("Progress exported to progress_report.txt")



# --- Future Expansion Hooks (commented for now) ---
#
# def update_user_xp(username, points):
#     """Increase XP for user, and handle level-ups later."""
#     pass
#
# def unlock_achievement(username, achievement_name):
#     """Record a new achievement unlocked by the user."""
#     pass
#
# def get_user_profile(username):
#     """Fetch overall user progress stats."""
#    