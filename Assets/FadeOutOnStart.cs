using UnityEngine;

public class FadeOutOnStart : MonoBehaviour
{
    public float fadeDuration = 0.5f;

    private CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();

        // Ensure it starts fully black
        if (cg != null)
            cg.alpha = 1f;
    }

    private void Start()
    {
        if (cg != null)
            StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        cg.alpha = 0f;
    }
}
