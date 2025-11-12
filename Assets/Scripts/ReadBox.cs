using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions; // <--- Add this

public class ReadBox : MonoBehaviour
{
    public TMP_InputField input;
    public Movement playerMovement;
    public string inputText;
    public List<string> textLines = new List<string>();
    public bool failed = false;
    public TextMeshProUGUI[] codeLines;

    void Start()
    {
        //input.onEndEdit.AddListener(delegate { ReadBoxInput(); });
        //codeLines.text = "1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n";
    }

    public void ReadBoxInput()
    {
        inputText = input.text;
        string[] lines = inputText.Split('\n');
        textLines.Clear();

        foreach (string line in lines)
        {
            textLines.Add(line.Trim());
        }

        StartCoroutine(PerformActions());
    }

    IEnumerator PerformActions()
    {
        failed = false;
        playerMovement.ChangeSprite(0);
        int counter = 0;
        codeLines[counter].color = Color.yellow;
        foreach (string line in textLines)
        {
            //changes color
            if(counter > 0){
                codeLines[counter - 1].color = Color.white;
                codeLines[counter].color = Color.yellow;
            }
            counter += 1;
            Debug.Log("READ: " + line);

            // player.x += #
            Match addMatch = Regex.Match(line, @"player\.x\s*\+=\s*(-?\d+(\.\d+)?)");
            if (addMatch.Success)
            {
                float amount = float.Parse(addMatch.Groups[1].Value);
                playerMovement.MoveRight(amount);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.x -= #
            Match subMatch = Regex.Match(line, @"player\.x\s*\-=\s*(-?\d+(\.\d+)?)");
            if (subMatch.Success)
            {
                float amount = float.Parse(subMatch.Groups[1].Value);
                playerMovement.MoveLeft(amount);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.Jump(#)
            Match jumpMatch = Regex.Match(line, @"player\.Jump\s*\(\s*(-?\d+(\.\d+)?)\s*\)");
            if (jumpMatch.Success)
            {
                float height = float.Parse(jumpMatch.Groups[1].Value);
                playerMovement.Jump(height);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.JumpRight(distance, height)
            Match jrMatch = Regex.Match(line, @"player\.JumpRight\s*\(\s*(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)\s*\)");
            if (jrMatch.Success)
            {
                float distance = float.Parse(jrMatch.Groups[1].Value);
                float height = float.Parse(jrMatch.Groups[3].Value);
                playerMovement.JumpRight(distance, height);
                yield return new WaitForSeconds(1f);
                continue;
            }

            // player.JumpLeft(distance, height)
            Match jlMatch = Regex.Match(line, @"player\.JumpLeft\s*\(\s*(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)\s*\)");
            if (jlMatch.Success)
            {
                float distance = float.Parse(jlMatch.Groups[1].Value);
                float height = float.Parse(jlMatch.Groups[3].Value);
                playerMovement.JumpLeft(distance, height);
                yield return new WaitForSeconds(1f);
                continue;
            }

            Match setX = Regex.Match(line, @"player\.x\s*=\s*(-?\d+(\.\d+)?)");
            if(setX.Success){
                float x = float.Parse(setX.Groups[1].Value);
                playerMovement.SetX(x);
                yield return new WaitForSeconds(1f);
                continue;
            }
            // If no command matched, wait for 1 second
            playerMovement.ChangeSprite(3);
            codeLines[counter-1].color = Color.red;
            

            yield return new WaitForSeconds(1f);
            failed = true;
            break;
        }
        if(!failed)
        playerMovement.ChangeSprite(2);
        
        yield return new WaitForSeconds(1f);
        playerMovement.ChangeSprite(1);

        foreach(var codeLine in codeLines){
            codeLine.color = Color.white;
        }
    }
    
}
