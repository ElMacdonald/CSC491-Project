using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelCompletion : MonoBehaviour
{
    [Header("Level Settings")]
    public int thisLevelIndex = 0;

    public string levelSelectSceneName = "LevelSelect";

    public string nextLevelSceneName = "";

    public void CompleteAndReturn()
    {
        MarkComplete();
        SceneManager.LoadScene(levelSelectSceneName);
    }


    public void CompleteAndLoadNext()
    {
        MarkComplete();

        if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
            SceneManager.LoadScene(nextLevelSceneName);
        }
        else
        {
            // No next level set, so just go back to level select
            SceneManager.LoadScene(levelSelectSceneName);
        }
    }

    // Just marks the level as done without loading any scene.
    public void MarkComplete()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.CompleteLevel(thisLevelIndex);
        }
        else
        {
            Debug.LogWarning("LevelCompletion: Couldn't find a LevelManager — did you add one to your scene?");
        }
    }

}
