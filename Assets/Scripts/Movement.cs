using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    private string moveState = "idle";
    private SpriteRenderer spr;
    public Sprite[] sprites;

    public Sprite[] rightWalkingSprites;
    public Sprite[] leftWalkingSprites;
    public Sprite[] jumpingSprites;
    public Sprite[] idleSprites;

    public float jumpTimer;
    public float jumpDelay;

    public float landDelay = .1f;
    public float landTimer;

    public float timeBetweenFrames = 0.1f;
    public float timer = 0f;
    public int currentFrame = 0;

    public float floatAmplitude = 0.05f;
    public float floatFrequency = 6f;
    private Vector3 basePos;
    public bool canTele=true;

    void Start(){
        spr = GetComponent<SpriteRenderer>();
        basePos = transform.position;
    }

    void Update(){
        jumpTimer += Time.deltaTime;
        handleAnims();
        if(moveState != "jumping"){
            landTimer += Time.deltaTime;
            if(landTimer >= landDelay){
            Vector3 p = basePos;
            p.y += Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, p.y, transform.position.z);
            }
        }else{
            landTimer = 0f;
        }
        

        Ray2D ray = new Ray2D(transform.position, Vector2.down);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 2f);
        Debug.DrawRay(ray.origin, ray.direction * 1.5f, Color.red);

        if(hit.collider != null && hit.collider.tag == "Ground" && moveState == "jumping" && jumpTimer > jumpDelay){
            moveState = "idle";
            timer = 1;
        }
    }

    public void handleAnims(){
        timer += Time.deltaTime;
        if(timer >= timeBetweenFrames){
            timer = 0f;
            currentFrame = (currentFrame + 1) % 4;
            switch(moveState){
                case "left walk":
                    spr.sprite = leftWalkingSprites[currentFrame];
                    break;
                case "right walk":
                    spr.sprite = rightWalkingSprites[currentFrame];
                    break;
                case "jumping":
                    spr.sprite = jumpingSprites[currentFrame];
                    break;
                case "idle":
                    spr.sprite = idleSprites[currentFrame];
                    break;
            }
        }
    }

    public void MoveLeft(float distance){
        moveState = "left walk";
        StartCoroutine(MoveCharacter(Vector3.left, distance));
    }

    public void MoveRight(float distance){
        moveState = "right walk";
        StartCoroutine(MoveCharacter(Vector3.right, distance));
    }

    public void SetX(float x){
        if(canTele){
            Vector3 pos = transform.position;
            pos.x = x;
            transform.position = pos;
        }
    }

    public void JumpRight(float distance, float height){
        StartCoroutine(MoveCharacter(Vector3.right, distance));
        Jump(height);
    }

    public void JumpLeft(float distance, float height){
        StartCoroutine(MoveCharacter(Vector3.left, distance));
        Jump(height);
    }

    private IEnumerator MoveCharacter(Vector3 direction, float distance){
        float moved = 0f;
        timer = 1f; //force frame change
        float speed = distance;
        while(moved < distance){
            float step = speed * Time.deltaTime;
            transform.Translate(direction * step);
            moved += step;
            yield return null;
        }
        moveState = "idle";
    }

    public void Jump(float height){
        jumpTimer = 0f;
        timer = 1f;
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, height), ForceMode2D.Impulse);
        moveState = "jumping";
    }
}
