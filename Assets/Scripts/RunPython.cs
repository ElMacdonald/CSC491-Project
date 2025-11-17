using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PythonRunner : MonoBehaviour
{
    public string pythonExePath = @"C:\Users\kylem\AppData\Local\Programs\Python\Python314\python.exe";
    public string pythonScriptPath = @"D:\Unity\CSC491-Project\Assets\Resources\Python\inputs_demo.py";

    public void RunPython()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = pythonExePath;  // MUST be python.exe
            psi.Arguments = $"\"{pythonScriptPath}\"";  // the script you want python to run
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            Debug.Log("Starting Python process...");

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Debug.Log("PYTHON OUTPUT:\n" + output);

                if (!string.IsNullOrEmpty(error))
                    Debug.LogError("PYTHON ERROR:\n" + error);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to run Python: " + e.Message);
        }
    }
}
