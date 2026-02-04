using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateMenuGuy : MonoBehaviour
{
    public Sprite[] sprites;
    public Image spr;
    public float animInterval = .4f;
    public float animTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnims();
    }

    void HandleAnims()
    {
        animTimer += Time.deltaTime;
        if(animTimer >= animInterval)
        {
            animTimer = 0f;
            Sprite currentSprite = spr.sprite;
            int currentIndex = System.Array.IndexOf(sprites, currentSprite);
            int nextIndex = (currentIndex + 1) % sprites.Length;
            spr.sprite = sprites[nextIndex];
        }
    }
}
