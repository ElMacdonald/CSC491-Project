using UnityEngine;
using TMPro;
using System.IO;

public class TextFileReader : MonoBehaviour
{
    [Header("UI Text Component")]
    public TextMeshProUGUI textDisplay;

    [Header("Full file path here")]
    public string filePath;

    public GameObject panel;
    void Start()
    {
        //LoadTextFile();
    }

    public void LoadTextFile()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("File path is empty.");
            return;
        }

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
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
