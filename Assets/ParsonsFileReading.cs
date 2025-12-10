using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ParsonsFileReading : MonoBehaviour
{
    public GameObject[] dropZones;

    private string filePath;

    private void Awake()
    {
        // Works on all platforms
        filePath = Path.Combine(Application.dataPath, "Stuff/Python/player_input.txt");

        // Ensure file exists
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }
    }

    // -----------------------------
    // WRITE DROPZONE TEXT TO FILE
    // -----------------------------
    public void WriteDropzones()
    {
        try
        {
            List<string> lines = new List<string>();

            foreach (GameObject zone in dropZones)
            {
                TextMeshProUGUI tmp = zone.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null)
                {
                    lines.Add(tmp.text);
                }
                else
                {
                    lines.Add(""); // keep line count consistent
                }
            }

            File.WriteAllLines(filePath, lines);
            Debug.Log("Wrote text to: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error writing file: " + ex.Message);
        }
    }

    // -----------------------------
    // READ FILE INTO DROPZONES
    // -----------------------------
    public void ReadDropzones()
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < dropZones.Length && i < lines.Length; i++)
            {
                TextMeshProUGUI tmp = dropZones[i].GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.text = lines[i];
                }
            }

            Debug.Log("Read text from: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error reading file: " + ex.Message);
        }
    }
}
