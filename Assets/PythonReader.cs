using System.Diagnostics;
using UnityEngine;
using System.IO;

public class RunPythonFile : MonoBehaviour
{
    [Header("Path to your Python file")]
    public string pythonFilePath = "C:/path/to/your/script.py";

    void Start()
    {
        RunPythonScript();
    }

    public void RunPythonScript()
    {
        if (!File.Exists(pythonFilePath))
        {
            UnityEngine.Debug.LogError("Python file not found: " + pythonFilePath);
            return;
        }

        // Set up process start info
        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = "python"; // or "python3" if needed
        psi.Arguments = $"\"{pythonFilePath}\"";
        psi.UseShellExecute = false;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.CreateNoWindow = true;

        try
        {
            Process process = Process.Start(psi);

            // Read the standard output and error
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrEmpty(output))
                UnityEngine.Debug.Log("Python output:\n" + output);

            if (!string.IsNullOrEmpty(errors))
                UnityEngine.Debug.LogError("Python errors:\n" + errors);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Error running Python script: " + e.Message);
        }
    }
}
