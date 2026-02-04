using UnityEngine;
using TMPro;
using System.IO;

public class TextFileReader : MonoBehaviour
{
    [Header("UI Text Component")]
    public TextMeshProUGUI textDisplay;

    [Header("Panel to show text")]
    public GameObject panel;

    private string filePath;   // auto-generated

    void Awake()
    {
        // Automatically builds the correct full path on any computer
        filePath = Path.Combine(Application.dataPath, "Stuff/Python/ai_feedback.txt");
    }

    public void LoadTextFile()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found at: " + filePath);
            return;
        }

    try
    {
        string contents = File.ReadAllText(filePath);

        // Find the "Feedback:" section
        string searchTerm = "Feedback:";
        int index = contents.IndexOf(searchTerm);

        if (index != -1)
        {
            // Extract everything after "Feedback:"
            string afterFeedback = contents.Substring(index + searchTerm.Length).Trim();

            textDisplay.text = afterFeedback;
        }
        else
        {
            Debug.LogWarning("Could not find 'Feedback:' in the file.");
            textDisplay.text = contents; // fallback
        }
    }
        catch (System.Exception ex)
        {
            Debug.LogError("Error reading file: " + ex.Message);
        }

        //panel.SetActive(true);

    
    }
    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}