using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomCamera : MonoBehaviour
{ 
    [SerializeField] private bool followPlayer;

    private CinemachineVirtualCamera _cam;
    private RoomManager _myRoom;

    void Start()
    {
        _myRoom = transform.parent.GetComponent<RoomManager>();
        _cam = GetComponent<CinemachineVirtualCamera>();
        if (followPlayer)
            _cam.Follow = PlayerManager.Instance.transform;
        Deactivate();
    }
    
    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += OnStateChange;
        RoomFactory.OnActivateRoom += OnRoomActivation;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= OnStateChange;
        RoomFactory.OnActivateRoom -= OnRoomActivation;
    }

    void OnStateChange(GameState state)
    { if (state == GameState.BetweenRooms) Deactivate();}


    void OnRoomActivation(RoomManager triggeredRoom)
    { if (triggeredRoom == _myRoom) Activate(); }

    private void Activate()
    {
        _cam.enabled = true;
        _cam.Priority = 100;
    }

    private void Deactivate()
    {
        _cam.enabled = false;
        _cam.Priority = -100;
    }
}