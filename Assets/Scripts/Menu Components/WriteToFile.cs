using UnityEngine;
using TMPro;


// Application.dataPath is a server URL in WebGL and cannot be written to.
public class TMPToFileWriter : MonoBehaviour
{
    [Header("TMP Text Component")]
    public TextMeshProUGUI textSource;

    public void SaveTextToFile()
    {
        if (textSource == null)
        {
            Debug.LogError("[TMPToFileWriter] TextMeshProUGUI source is not assigned.");
            return;
        }

        PythonInputStore.text = textSource.text;
        Debug.Log("[TMPToFileWriter] Saved text to PythonInputStore.");
    }
}
