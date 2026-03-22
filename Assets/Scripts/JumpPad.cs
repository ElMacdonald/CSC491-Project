using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public int jumpHeight;
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Movement m = other.gameObject.GetComponent<Movement>();
            if(m != null){
                m.Jump(jumpHeight);
            }
        }
    }
}
