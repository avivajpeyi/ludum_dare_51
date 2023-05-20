using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomFactory : Singleton<RoomFactory>
{
    public int startingNumRooms = 5;

    [SerializeField] private RoomManager startingRoomManager;
    [SerializeField] private RoomSet roomSet;

    private LinkedList<RoomManager> _rooms;

    private int maxNumRooms = 10;

    public static int SpawnCount
    {
        get;
        private set;
    }


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
        if (newState == GameState.InRoom  && oldState == GameState.BetweenRooms)
        {
            SpawnRoom();
            CleanUpRooms();
        }
    }

    private void CleanUpRooms()
    {
        if (_rooms.Count > maxNumRooms)
        {
            RoomManager lastRoom = _rooms.Last.Value;
            Destroy(lastRoom.gameObject);
            _rooms.RemoveLast();
        }
    }

    private void SpawnRoom()
    {
        SpawnCount++;
        GameObject roomPrefab = roomSet.GetRandomRoom();
        RoomManager oldRoom = NewestRoom;
        RoomManager newRoom =
            Instantiate(roomPrefab, oldRoom.GetEndPoint(), Quaternion.identity)
                .GetComponent<RoomManager>();
        newRoom.AlignToPreviousRoom(oldRoom);
        _rooms.AddFirst(newRoom);
        newRoom.transform.parent = this.transform;
        newRoom.transform.name = $"Room{SpawnCount}";
    }
}