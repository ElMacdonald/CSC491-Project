using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsonDeclan : MonoBehaviour
{
    public GameObject dropArea;
    public string targetName = "CodeLine1";
    public shipMovement playerShip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if drop area has a child named codeline1, set player damage to 10
        if (dropArea.transform.Find(targetName) != null)
        {
            playerShip.power = 10f;
        }
        else
        {
            playerShip.power = 1f;
        }
    }
}
