using UnityEngine;
using TMPro;
using System.IO;

public class TMPToFileWriter : MonoBehaviour
{

    
    [Header("TMP Text Component")]
    public TextMeshProUGUI textSource;

    // Automatically generated file path
    private string filePath;

    private void Awake()
    {
        // Build the correct path on ANY computer
        filePath = Path.Combine(Application.dataPath, "Stuff/Python/player_input.txt");
    }

    public void SaveTextToFile()
    {
        if (textSource == null)
        {
            Debug.LogError("TextMeshProUGUI source is not assigned.");
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
