using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Fade Panel")]
    public CanvasGroup fadePanel;   // Drag your fade panel CanvasGroup here
    public float fadeDuration = 0.5f;

    public void LoadLevelByIndex(int index)
    {
        StartCoroutine(FadeAndLoad(index));
    }

    private IEnumerator FadeAndLoad(int index)
    {
        // Fade to black
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 1f;

        // Load the scene
        SceneManager.LoadScene(index);
    }
}
