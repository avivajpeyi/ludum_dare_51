using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderReset : MonoBehaviour
{
    Sprite currentSprite;
    BoxCollider2D coll;
    SpriteRenderer spr;

    void Start() {
        coll = gameObject.GetComponentInChildren<BoxCollider2D>();
        spr = gameObject.GetComponentInChildren<SpriteRenderer>();
        UpdateCollider();
    }

    void Update()
    {
        if(currentSprite != spr.sprite)
        {
            currentSprite = spr.sprite;
            UpdateCollider();
        }
    }

    public void UpdateCollider()
    {
        coll.size = spr.sprite.bounds.size;
        coll.offset = Vector2.zero;

    }
}
