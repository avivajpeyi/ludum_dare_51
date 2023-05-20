using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomFactory : Singleton<RoomFactory>
{
    public int startingNumRooms = 2;
    [SerializeField] private int winRoomNumber = 50;
    [SerializeField] private RoomManager startingRoomManager;
    [SerializeField] private RoomSet roomSet;


    private LinkedList<RoomManager> _rooms;

    private readonly int maxNumRooms = 3;

    public static int SpawnCount { get; private set; }

    int CurrentNumberOfRooms => _rooms.Count;

    bool IsWinningRoom => SpawnCount == winRoomNumber;
    private bool winningRoomSpawned = false;


    RoomManager NewestRoom
    {
        get { return _rooms.First.Value; }
    }


    private void Start()
    {
        _rooms = new LinkedList<RoomManager>();
        _rooms.AddFirst(startingRoomManager);
        SpawnCount = 0;
        for (int i = 0; i < startingNumRooms; i++)
        {
            SpawnRoom();
        }
    }

    private void OnEnable()
    {
        GameEventManager.OnBeforeStateChanged += OnStateChange;
    }

    private void OnDisable()
    {
        GameEventManager.OnBeforeStateChanged -= OnStateChange;
    }


    void OnStateChange(GameState oldState, GameState newState)
    {
        if (newState == GameState.InRoom && oldState == GameState.BetweenRooms)
        {
            SpawnRoom();
            CleanUpRooms();
        }
        else if (newState == GameState.StartingGame)
            SpawnCount = 0;
    }

    private void CleanUpRooms()
    {
        if (CurrentNumberOfRooms > maxNumRooms)
        {
            RoomManager lastRoom = _rooms.Last.Value;
            Destroy(lastRoom.gameObject);
            _rooms.RemoveLast();
        }
    }

    private void SpawnRoom()
    {
        if (winningRoomSpawned) return; // no more rooms to spawn

        SpawnCount++;
        RoomManager oldRoom = NewestRoom;
        RoomManager newRoom = roomSet.SpawnRoom(oldRoom, IsWinningRoom);
        _rooms.AddFirst(newRoom);
        newRoom.transform.parent = this.transform;
        newRoom.transform.name = $"Room{SpawnCount}";
        if (IsWinningRoom)
        {
            newRoom.transform.name = $"RoomEnd";
            winningRoomSpawned = true;
        }

     
    }
}