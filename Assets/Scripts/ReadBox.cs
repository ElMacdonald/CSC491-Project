using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class ReadBox : MonoBehaviour
{
    public TMP_InputField input;
    public Movement playerMovement;
    public string inputText;
    public List<string> textLines = new List<string>();
    public bool failed = false;
    public TextMeshProUGUI[] codeLines;

    public TextFileReader ts; //pmo

    public List<string> variableNames = new List<string>();
    public List<float> variableValues = new List<float>();
    public ObjectiveTracker objTracker;

    void Start()
    {
        // input.onEndEdit.AddListener(delegate { ReadBoxInput(); });
        objTracker = GameObject.Find("Objective Manager").GetComponent<ObjectiveTracker>();
    }

    public void ReadBoxInput()
    {   
        inputText = input.text;
        string[] lines = inputText.Split('\n');
        textLines.Clear();

        foreach (string line in lines)
            textLines.Add(line.Trim());

        StartCoroutine(PerformActions());
    }

    //Checks whether provided value is var or num
    float ResolveValue(string token)
    {
        if (float.TryParse(token, out float number))
            return number;

        int index = variableNames.IndexOf(token);
        if (index != -1)
        {
            Debug.Log("Var used");
            objTracker.usedVars += 1;
            return variableValues[index];
        }
            

        Debug.LogError("Unknown variable: " + token);
        failed = true;
        return 0f;
    }

    IEnumerator PerformActions()
    {
        variableNames.Clear();
        variableValues.Clear();
        failed = false;

        int counter = 0;
        codeLines[counter].color = Color.yellow;

        foreach (string line in textLines)
        {
            if (counter > 0)
            {
                codeLines[counter - 1].color = Color.white;
                codeLines[counter].color = Color.yellow;
            }

            counter += 1;
            Debug.Log("READ: " + line);

            // player.x += value
            Match addMatch = Regex.Match(line,
                @"player\.x\s*\+=\s*([a-zA-Z_][a-zA-Z0-9_]*|-?\d+(\.\d+)?)");
            if (addMatch.Success)
            {
                float amount = ResolveValue(addMatch.Groups[1].Value);
                playerMovement.MoveRight(amount);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.x -= value
            Match subMatch = Regex.Match(line,
                @"player\.x\s*\-=\s*([a-zA-Z_][a-zA-Z0-9_]*|-?\d+(\.\d+)?)");
            if (subMatch.Success)
            {
                float amount = ResolveValue(subMatch.Groups[1].Value);
                playerMovement.MoveLeft(amount);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.Jump(value)
            Match jumpMatch = Regex.Match(line,
                @"player\.Jump\s*\(\s*([a-zA-Z_][a-zA-Z0-9_]*|-?\d+(\.\d+)?)\s*\)");
            if (jumpMatch.Success)
            {
                float height = ResolveValue(jumpMatch.Groups[1].Value);
                playerMovement.Jump(height);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.JumpRight(distance, height)
            Match jrMatch = Regex.Match(line,
                @"player\.JumpRight\s*\(\s*([a-zA-Z_][a-zA-Z0-9_]*|-?\d+(\.\d+)?),\s*([a-zA-Z_][a-zA-Z0-9_]*|-?\d+(\.\d+)?)\s*\)");
            if (jrMatch.Success)
            {
                float distance = ResolveValue(jrMatch.Groups[1].Value);
                float height = ResolveValue(jrMatch.Groups[3].Value);
                playerMovement.JumpRight(distance, height);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.JumpLeft(distance, height)
            Match jlMatch = Regex.Match(line,
                @"player\.JumpLeft\s*\(\s*([a-zA-Z_][a-zA-Z0-9_]*|-?\d+(\.\d+)?),\s*([a-zA-Z_][a-zA-Z0-9_]*|-?\d+(\.\d+)?)\s*\)");
            if (jlMatch.Success)
            {
                float distance = ResolveValue(jlMatch.Groups[1].Value);
                float height = ResolveValue(jlMatch.Groups[3].Value);
                playerMovement.JumpLeft(distance, height);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.x = value
            Match setX = Regex.Match(line,
                @"player\.x\s*=\s*([a-zA-Z_][a-zA-Z0-9_]*|-?\d+(\.\d+)?)");
            if (setX.Success)
            {
                float x = ResolveValue(setX.Groups[1].Value);
                playerMovement.SetX(x);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.y = value
            Match setY = Regex.Match(line,
                @"player\.y\s*=\s*([a-zA-Z_][a-zA-Z0-9_]*|-?\d+(\.\d+)?)");
            if (setY.Success)
            {
                float y = ResolveValue(setY.Groups[1].Value);
                playerMovement.SetY(y);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // varName = number
            Match setVar = Regex.Match(line,
                @"([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(-?\d+(\.\d+)?)");
            if (setVar.Success)
            {
                string varName = setVar.Groups[1].Value;
                float varValue = float.Parse(setVar.Groups[2].Value);

                int index = variableNames.IndexOf(varName);
                if (index != -1)
                    variableValues[index] = varValue;
                else
                {
                    variableNames.Add(varName);
                    variableValues.Add(varValue);
                }

                Debug.Log("Set variable " + varName + " to " + varValue);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // Invalid line
            codeLines[counter - 1].color = Color.red;
            failed = true;
            yield return new WaitForSeconds(1f);
            break;
        }

        foreach (var codeLine in codeLines)
            codeLine.color = Color.white;
    }
}
