using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomFactory : Singleton<RoomFactory>
{
    private readonly int maxNumRooms = 3;
    public static int SpawnCount { get; private set; }

    int CurrentNumberOfRooms => _rooms.Count;
    RoomManager NewestRoom => _rooms.First.Value;


    bool IsWinningRoom => SpawnCount == winRoomNumber;


    public int startingNumRooms = 2;
    [SerializeField] private int winRoomNumber = 50;
    [SerializeField] private RoomSet roomSet;
    [SerializeField] private RoomManager startRoom;
    RoomManager ActiveRoom { get; set; }
    RoomManager PreviousRoom { get; set; }
    private LinkedList<RoomManager> _rooms;


    private bool winningRoomSpawned = false;

    public static int ActiveRoomNumber { get; private set; }
    public static string ActiveRoomName => Instance.ActiveRoom.name;


    public static event Action<RoomManager> OnActivateRoom;
    public static event Action<RoomManager> OnFinishRoom;


    private void Start()
    {
        _rooms = new LinkedList<RoomManager>();
        _rooms.AddFirst(startRoom);
        SpawnCount = 0;
        for (int i = 0; i < startingNumRooms; i++)
            SpawnRoom();
    }

    private void OnEnable()
    {
        GameManager.OnBeforeStateChanged += OnStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnBeforeStateChanged -= OnStateChange;
    }


    void OnStateChange(GameState oldState, GameState newState)
    {
        if (newState == GameState.InRoom && oldState == GameState.BetweenRooms)
        {

            SpawnRoom();
            CleanUpRooms();
        }
        else if (newState == GameState.StartingGame)
        {
            SpawnCount = 0;
            ActiveRoomNumber = 0;
        }
    }

    public void TriggerActivateRoom(RoomManager room)
    {
        Debug.Log("Activating " + room.name);
        ActiveRoom = room;
        ActiveRoomNumber++;
        OnActivateRoom?.Invoke(room);
        GameManager.Instance.ChangeState(GameState.InRoom);
    }


    public void TriggerFinishRoom()
    {
        Debug.Log("Finished " + ActiveRoom.name);
        OnFinishRoom?.Invoke(ActiveRoom);
        PreviousRoom = ActiveRoom;
        ActiveRoom = null;
        GameManager.Instance.ChangeState(GameState.BetweenRooms);
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