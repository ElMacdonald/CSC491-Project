using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablisWin : MonoBehaviour
{
     public GameObject dropArea1;
    public string targetName1 = "CodeLine1";
    public GameObject dropArea2;
    public string targetName2 = "CodeLine2";
    public shipMovement playerShip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if drop area has a child named codeline1, set player damage to 10
        if (dropArea1.transform.Find(targetName1) != null && dropArea2.transform.Find(targetName2) != null)
        {
            playerShip.power = 10f;
        }
        else
        {
            playerShip.power = 1f;
        }
    }
}
