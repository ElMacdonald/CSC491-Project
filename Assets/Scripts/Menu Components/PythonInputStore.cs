// Shared in-memory store for player Python input.
// Replaces file I/O (Application.dataPath writes) so the game works in WebGL.
// ParsonsFileReading.cs and WriteToFile.cs both read/write here.
public static class PythonInputStore
{
    public static string[] lines = new string[0];
    public static string text = "";
}
