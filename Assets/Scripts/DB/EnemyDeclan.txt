using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeclan : MonoBehaviour
{
    public shipMovement shipMovement;
    public GameObject winPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Projectile")
        {
            if(shipMovement.power == 10)
            {
                winPanel.SetActive(true);
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}
