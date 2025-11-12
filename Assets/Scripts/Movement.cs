using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    private bool midMovement = false;
    private SpriteRenderer spr;
    public Sprite[] sprites;



    void Start(){
        spr = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(int index){
        spr.sprite = sprites[index];
    }
    //Starts coroutine to move character to the left for 1 second
    public void MoveLeft(float distance){
        midMovement = true;
        spr.flipX = true;
        StartCoroutine(MoveCharacter(Vector3.left, distance));
    }
    //Starts coroutine to move character to the right for 1 second
    public void MoveRight(float distance){
        spr.flipX = false;
        midMovement = true;
        StartCoroutine(MoveCharacter(Vector3.right, distance));
    }

    public void SetX(float x){
        Vector3 pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }

    public void JumpRight(float distance, float height){
        midMovement = true;
        spr.flipX = false;
        StartCoroutine(MoveCharacter(Vector3.right, distance));
        Jump(height);
    }

    public void JumpLeft(float distance, float height){
        midMovement = true;
        spr.flipX = true;
        StartCoroutine(MoveCharacter(Vector3.left, distance));
        Jump(height);
    }

    private IEnumerator MoveCharacter(Vector3 direction, float distance){
        float moved = 0f;
        float speed = distance; // Move the entire distance in 1 second
        while(moved < distance){
            float step = speed * Time.deltaTime;
            transform.Translate(direction * step);
            moved += step;
            yield return null;
        }
        midMovement = false;
    }

    public void Jump(float height){
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, height), ForceMode2D.Impulse);
    }

}
