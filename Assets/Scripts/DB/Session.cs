using UnityEngine;

public static class Session
{
    public static string     userId        = "";
    public static string     classroomCode = "";
    public static PlayerData currentPlayer = null;

    private static float _sessionStartTime = 0f;

    public static void StartSession()
    {
        _sessionStartTime = Time.realtimeSinceStartup;
        if (currentPlayer != null)
        {
            currentPlayer.totalSessions++;
            currentPlayer.lastLogin = System.DateTime.UtcNow.ToString("o");
        }
    }

    // Accumulates play time before each Firebase save
    public static void FlushPlayTime()
    {
        if (currentPlayer != null && _sessionStartTime > 0f)
        {
            currentPlayer.totalPlayTime += Time.realtimeSinceStartup - _sessionStartTime;
            _sessionStartTime = Time.realtimeSinceStartup;
        }
    }
}
