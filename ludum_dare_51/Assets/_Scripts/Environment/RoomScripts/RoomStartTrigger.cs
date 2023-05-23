using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomStartTrigger : MonoBehaviour
{
    private RoomFactory _roomFactory;
    private RoomDoorController _start;
    private RoomManager _myRoom;

    private void Awake()
    {
        _start = GetComponent<RoomDoorController>();
        if (name == "EndPosition") Destroy(this);
        _myRoom = transform.parent.GetComponent<RoomManager>();
        if (_myRoom == null) throw new Exception("RoomStartTrigger must be a child of a RoomManager");
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _start.stayClosed = true;
            RoomFactory.Instance.TriggerActivateRoom(_myRoom);
            Destroy(this);
        }
    }
    
}