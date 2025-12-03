using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite spriteLeft;
    public Sprite spriteIdle;
    public Sprite spriteRight;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            spriteRenderer.sprite = spriteLeft;
        }else if(Input.GetKey(KeyCode.RightArrow)){
            spriteRenderer.sprite = spriteRight;
        }else{
            spriteRenderer.sprite = spriteIdle;
        }
    }
}
