using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private const string LEVEL_KEY_PREFIX = "Level_Completed_";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void CompleteLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(LEVEL_KEY_PREFIX + levelIndex, 1);
        PlayerPrefs.Save();
        Debug.Log($"Level {levelIndex} marked as completed.");
    }


    public bool IsLevelCompleted(int levelIndex)
    {
        return PlayerPrefs.GetInt(LEVEL_KEY_PREFIX + levelIndex, 0) == 1;
    }

    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All level progress has been reset.");
    }


    public void ResetLevel(int levelIndex)
    {
        PlayerPrefs.DeleteKey(LEVEL_KEY_PREFIX + levelIndex);
        PlayerPrefs.Save();
    }
}
