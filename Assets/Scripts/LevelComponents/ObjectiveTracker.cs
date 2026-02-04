using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectiveTracker : MonoBehaviour
{
    public GameObject collectablesParent;

    public bool collectableLevel;
    public int totalCollectables;
    public int currentCollectables = 0;

    public bool varsNeeded;
    public int usedVars;
    public int neededVars;

    public bool levelWon = false;


    public TextMeshProUGUI objectiveDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Counts gameobjects under collectableParent to get total collectables
        if(collectableLevel){
            totalCollectables = collectablesParent.transform.childCount;
        }
        UpdateObjectiveDisplay();
    }

    // Lists all objectives if they're active
    void UpdateObjectiveDisplay(){
        List<string> objectives = new List<string>();

        if(collectableLevel){
            // Checks amount of ACTIVE child objects to get current collectables
            currentCollectables = totalCollectables;
            foreach(Transform child in collectablesParent.transform){
                if(child.gameObject.activeSelf){
                    currentCollectables -= 1;
                }
            }
            objectives.Add("Collectables: " + currentCollectables + " / " + totalCollectables);
        }
        if(varsNeeded){
            objectives.Add("Variables Used: " + usedVars + " / " + neededVars);
        }

        objectiveDisplay.text = string.Join("\n", objectives);
    }

    void checkWin()
    {
        if(collectableLevel){
            if(currentCollectables >= totalCollectables){
                levelWon = true;
            }
            else
            {
                levelWon = false;
                return;
            }
        }
        if(varsNeeded){
            if(usedVars >= neededVars){
                levelWon = true;
            }
            else
            {
                levelWon = false;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObjectiveDisplay();
        // Checks win conditions
        checkWin();
    }
}
