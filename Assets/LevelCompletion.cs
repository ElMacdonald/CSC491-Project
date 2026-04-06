using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletion : MonoBehaviour
{
    [Header("Level Settings")]
    public int    thisLevelIndex      = 0;
    public string levelSelectSceneName = "LevelSelect";
    public string nextLevelSceneName   = "";

    public void CompleteAndReturn()
    {
        MarkComplete();
        SceneManager.LoadScene(levelSelectSceneName);
    }

    public void CompleteAndLoadNext()
    {
        MarkComplete();
        SceneManager.LoadScene(!string.IsNullOrEmpty(nextLevelSceneName) ? nextLevelSceneName : levelSelectSceneName);
    }

    public void MarkComplete()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.CompleteLevel(thisLevelIndex);
    }
}
