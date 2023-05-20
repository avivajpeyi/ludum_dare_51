using System;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(menuName = "ScriptableObjects/RoomSet", fileName = "Create new room set")]

public class RoomSet : ScriptableObject
{
    [SerializeField] private List<GameObject> rooms;
    
    public GameObject GetRandomRoom()
    {
        return rooms[UnityEngine.Random.Range(0, rooms.Count)];
    }
}



[Serializable]
public enum RoomType
{
    TimedSurvival = 0, // Survive for X seconds
    ItemCollection = 1 // Collect X items
}




[CreateAssetMenu(fileName = "Create new room", menuName = "ScriptableObjects/Room")]
public class ScriptableRoom : ScriptableObject
{
    [SerializeField] private RoomType Type;
    
}



