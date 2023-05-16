using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelStartTrigger : MonoBehaviour
{
    private static int countRooms = 0;
    private GameEventManager _gameEventManager;
    private LevelUI _ui;
    private LerpToPosAtLevelStart _start;
    public bool levelStarted = false;
    BoxCollider2D myBoxCollider;
    
    private void Awake()
    {
        _start = GetComponent<LerpToPosAtLevelStart>();
        _gameEventManager = FindObjectOfType<GameEventManager>();
        if (name =="EndPosition") Destroy(this); // we only want this for StartPosition
        _ui = FindObjectOfType<LevelUI>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        // if player enters the trigger, start the level
        if (col.CompareTag("Player"))
        {
            _start.stayClosed = true;
            levelStarted = true;
            countRooms++;
            _ui.updateRoomNumber(countRooms);
            Debug.Log("Entered " + transform.root.name +
                      " (starting level, destroying col)");
            _gameEventManager.RunStartLevel();
            Destroy(this);
        }
    }
}
