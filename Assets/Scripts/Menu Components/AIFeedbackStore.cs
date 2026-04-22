// Shared in-memory store for AI-generated feedback.
// Replaces ai_feedback.txt file I/O so the game works in WebGL.
// AIEvaluator.cs writes here; TextFileReader (TextSetters.cs) reads from here.
public static class AIFeedbackStore
{
    public static string feedback = "";
}
