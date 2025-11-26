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
        filePath = Path.Combine(Application.dataPath, "Resources/Python/ai_feedback.txt");
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
            textDisplay.text = contents;
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
