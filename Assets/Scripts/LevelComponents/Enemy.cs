using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 6;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnColliderEnter2D(Collider2D other)
    {

        /*
        if(other.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if(projectile)
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
        }
        */
    }
}
