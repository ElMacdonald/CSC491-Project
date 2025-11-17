using UnityEngine;
using TMPro;
using System.IO;

public class TMPToFileWriter : MonoBehaviour
{
    [Header("TMP Text Component")]
    public TextMeshProUGUI textSource;

    [Header("Full output file path")]
    public string filePath;

    // Call this from a button or manually
    public void SaveTextToFile()
    {
        if (textSource == null)
        {
            Debug.LogError("TextMeshProUGUI source is not assigned.");
            return;
        }

        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("File path is empty.");
            return;
        }

        try
        {
            File.WriteAllText(filePath, textSource.text);
            Debug.Log("Saved text to: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error writing file: " + ex.Message);
        }
    }
}
