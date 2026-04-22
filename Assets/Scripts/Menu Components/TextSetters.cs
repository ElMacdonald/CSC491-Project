using UnityEngine;
using TMPro;
#if !UNITY_WEBGL
using System.IO;
#endif

// WebGL-safe feedback display.
// 
// DESKTOP behavior (unchanged):
//   The Submit button calls LoadTextFile() after RunPython() finishes (blocking),
//   so the file is already written. Works exactly as before.
//
// WEBGL behavior:
//   The Submit button should NOT call LoadTextFile() directly.
//   Instead, AIEvaluator.RunEvaluation() is called on the button,
//   which fires off the async Groq API call and then calls LoadTextFile()
//   itself once the response arrives.
//   While waiting, the panel shows "Thinking..." (set by AIEvaluator.statusText).

public class TextFileReader : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textDisplay;
    public GameObject panel;

#if !UNITY_WEBGL
    private string filePath;

    void Awake()
    {
        filePath = Path.Combine(Application.dataPath, "Stuff/Python/ai_feedback.txt");
    }
#endif

    // Called by:
    //   - The Submit button directly (desktop builds)
    //   - AIEvaluator.EvaluateCoroutine() after the API returns (WebGL builds)
    public void LoadTextFile()
    {
        string contents;

#if UNITY_WEBGL
        contents = AIFeedbackStore.feedback;
        if (string.IsNullOrEmpty(contents))
        {
            Debug.LogWarning("[TextFileReader] AIFeedbackStore is empty — AIEvaluator hasn't finished yet.");
            if (textDisplay != null)
                textDisplay.text = "Feedback not ready yet. Please wait a moment and try again.";
            return;
        }
#else
        if (!File.Exists(filePath))
        {
            Debug.LogError("[TextFileReader] File not found: " + filePath);
            return;
        }
        try
        {
            contents = File.ReadAllText(filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[TextFileReader] Error reading file: " + ex.Message);
            return;
        }
#endif

        // Extract and display the "Feedback:" section
        const string searchTerm = "Feedback:";
        int index = contents.IndexOf(searchTerm);
        if (textDisplay != null)
            textDisplay.text = index != -1
                ? contents.Substring(index + searchTerm.Length).Trim()
                : contents; // fallback: show full text

        if (index == -1)
            Debug.LogWarning("[TextFileReader] 'Feedback:' section not found in AI response.");
    }

    public void ClosePanel()
    {
        if (panel != null) panel.SetActive(false);
    }
}
