using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomStartTrigger : MonoBehaviour
{
    private GameEventManager _gameEventManager;
    private RoomDoorController _start;

    public static int CountRooms
    {
        get;
        private set;
    }

    private string RoomName => transform.parent.parent.name;

    private void Awake()
    {
        _start = GetComponent<RoomDoorController>();
        if (name == "EndPosition") Destroy(this); 
    }

    private void Start()
    {
        _gameEventManager = GameEventManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            CountRooms++;
            _start.stayClosed = true;
            _gameEventManager.ChangeState(GameState.InRoom);
            Debug.Log($"Entered Room {RoomName}");
            Destroy(this);
        }
    }
}