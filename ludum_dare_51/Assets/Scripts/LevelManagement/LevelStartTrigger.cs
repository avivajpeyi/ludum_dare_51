using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class LevelStartTrigger : MonoBehaviour
{
    private LevelTemplate currentLevel;
    private BoxCollider2D myCol;
    
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = transform.GetComponentInParent<LevelTemplate>();
        myCol = GetComponent<BoxCollider2D>();
        if (name =="EndPosition") Destroy(this); // we only want this for StartPosition

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // if player enters the trigger, start the level
        if (col.CompareTag("Player"))
        {
            currentLevel.StartLevel();
        }
        Destroy(this); // no more triggers possible
    }
}
