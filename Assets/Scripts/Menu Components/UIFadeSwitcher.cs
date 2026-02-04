using UnityEngine;
using System.Collections;

public class UIFadeSwitcher : MonoBehaviour
{
    [Header("Fade Panel (must have CanvasGroup)")]
    public CanvasGroup fadePanel;

    [Header("UI to enable AFTER fade-in")]
    public GameObject uiToEnable;

    [Header("Original UI to disable AFTER fade-in")]
    public GameObject originalUI;

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f;

    bool isFading = false;

    public void TriggerFade()
    {
        if (!isFading)
            StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        isFading = true;

        // Fade IN (black covers screen)
        yield return StartCoroutine(Fade(0f, 1f));

        // Disable old UI
        if (originalUI != null)
            originalUI.SetActive(false);

        // Enable new UI
        if (uiToEnable != null)
            uiToEnable.SetActive(true);

        // Fade OUT (reveal new UI)
        yield return StartCoroutine(Fade(1f, 0f));

        isFading = false;
    }

    IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            fadePanel.alpha = Mathf.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        fadePanel.alpha = end;
    }
}
