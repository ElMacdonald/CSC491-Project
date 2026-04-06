using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string userId        = "";
    public string displayName   = "";
    public string classroomCode = "";

    // Parallel lists used instead of Dictionary (JsonUtility limitation)
    public List<string> completedLevelIds = new List<string>();
    public List<int>    levelStars        = new List<int>();
    public List<int>    levelAttempts     = new List<int>();

    public int coins = 0;
    public List<string> unlockedCosmetics = new List<string>();
    public string equippedHat   = "";
    public string equippedShirt = "";
    public string equippedPants = "";

    public string lastLogin     = "";
    public float  totalPlayTime = 0f;
    public int    totalSessions = 0;

    public void MarkLevelComplete(string levelId, int stars = 1)
    {
        if (!completedLevelIds.Contains(levelId))
        {
            completedLevelIds.Add(levelId);
            levelStars.Add(stars);
            levelAttempts.Add(1);
        }
        else
        {
            int idx = completedLevelIds.IndexOf(levelId);
            if (stars > levelStars[idx]) levelStars[idx] = stars;
            levelAttempts[idx]++;
        }
    }

    public bool IsLevelComplete(string levelId) => completedLevelIds.Contains(levelId);

    public int GetStars(string levelId)
    {
        int idx = completedLevelIds.IndexOf(levelId);
        return idx >= 0 ? levelStars[idx] : 0;
    }
}
