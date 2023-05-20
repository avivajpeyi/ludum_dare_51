using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class RoomManager : MonoBehaviour
{
    private RoomManager prevRoomManager;
    private Transform endDoorTransform;
    private Transform startDoorTransform;
    private Transform playerReference;
    private Transform deathZone;
    private Light2D roomLight;


    private GameObject timer;
    private RoomDoorController startDoor;
    private RoomDoorController endDoor;


    void Awake()
    {
        SetReferences();
        endDoorTransform.GetComponent<SpriteRenderer>().enabled = false;
        startDoorTransform.GetComponent<SpriteRenderer>().enabled = false;
        deathZone.GetComponent<SpriteRenderer>().enabled = false;
        playerReference.gameObject.SetActive(false);
    }

    void SetReferences()
    {
        endDoorTransform = transform.Find("EndPosition");
        startDoorTransform = transform.Find("StartPosition");
        playerReference = transform.Find("PlayerReference");
        deathZone = transform.Find("DeathZone");
        roomLight = transform.Find("RoomLight").GetComponent<Light2D>();
        startDoor = startDoorTransform.GetComponentInChildren<RoomDoorController>();
        endDoor = endDoorTransform.GetComponentInChildren<RoomDoorController>();

        List<Transform> requiredTrans = new List<Transform>
        {
            endDoorTransform, startDoorTransform, playerReference, deathZone,
            roomLight.transform
        };
        if (!Helpers.AllTransformsExist(requiredTrans))
        {
            Debug.LogError(name + "missing assigment:" + requiredTrans);
        }
    }


    public void CloseLevelDoors()
    {
        startDoor.Close();
        endDoor.Close();
    }

    public void OpenExitDoor()
    {
        endDoor.Open();
        startDoor.Open();
    }

    private void OnEnable()
    {
        GameEventManager.OnAfterStateChanged += OnStateChange;
    }

    private void OnDisable()
    {
        GameEventManager.OnAfterStateChanged -= OnStateChange;
    }

    void OnStateChange(GameState state)
    {
        if (state == GameState.InRoom)
        {
            CloseLevelDoors();
        }
        else if (state == GameState.BetweenRooms)
        {
            OpenExitDoor();
        }
    }

    private void Start()
    {
        if (prevRoomManager != null)
            AlignToPreviousRoom(prevRoomManager);
        roomLight.intensity = 0.35f;
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            BoxCollider2D boxCollider2D = deathZone.GetComponent<BoxCollider2D>();
            Gizmos.DrawCube(boxCollider2D.bounds.center, boxCollider2D.bounds.size);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(GetEndPoint(), 0.5f);
            Gizmos.DrawSphere(GetStartPoint(), 0.5f);
        }
    }


    public Vector3 GetStartPoint()
    {
        return startDoorTransform.position;
    }

    public Transform GetEndTransform()
    {
        return endDoorTransform;
    }

    public Vector3 GetEndPoint()
    {
        return endDoorTransform.position;
    }

    public void AlignToPreviousRoom(RoomManager previous)
    {
        Transform curStart = startDoorTransform;
        Transform prevEnd = previous.GetEndTransform();
        prevRoomManager = previous;
        transform.position = prevEnd.position - curStart.localPosition;
    }
}