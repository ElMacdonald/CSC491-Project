using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;

public class PythonRunner : MonoBehaviour
{
    private string pythonExePath;
    private string pythonScriptPath;

    public int fileNumber = 1;  

    

    private void Awake()
    {
        pythonExePath = FindPythonExecutable();
        UnityEngine.Debug.Log(pythonExePath);
        pythonScriptPath = Path.Combine(Application.dataPath, "Stuff/Python/inputs.py");
    }

    public void RunPython()
    {
        if (pythonExePath == null)
        {
            UnityEngine.Debug.LogError("Python not found on this computer.");
            return;
        }

        try
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            
            pythonExePath = pythonExePath.Replace("\"", "");
            psi.FileName = pythonExePath;
            

            psi.Arguments = $"\"{pythonScriptPath}\" {fileNumber}";

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                UnityEngine.Debug.Log("PYTHON OUTPUT:\n" + output);

                if (!string.IsNullOrEmpty(error))
                    UnityEngine.Debug.LogError("PYTHON ERROR:\n" + error);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Failed to run Python: " + e.Message);
        }
    }

    private string FindPythonExecutable()
    {
        if (CommandExists("py"))
        {
            return "py";
        }

        if (CommandExists("python"))
            return "python";

        if (CommandExists("python3"))
            return "python3";

        string[] common = new[]
        {
            @"C:\Users\" + Environment.UserName + @"\AppData\Local\Programs\Python",
            @"C:\Python311",
            @"C:\Python312",
            @"C:\Python313",
            @"C:\Python314",
            @"C:\Program Files\Python311",
            @"C:\Program Files\Python312",
            @"C:\Program Files\Python313",
            @"C:\Program Files\Python314"
            
        };

        foreach (string root in common)
        {
            if (!Directory.Exists(root)) continue;

            foreach (string dir in Directory.GetDirectories(root))
            {
                string exe = Path.Combine(dir, "python.exe");
                if (File.Exists(exe))
                    return exe;
            }
        }

        return null; // Python not found
    }

    private bool CommandExists(string command)
    {
        try
        {
            Process p = new Process();
            p.StartInfo.FileName = command;
            p.StartInfo.Arguments = "--version";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.WaitForExit();

            return p.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
