using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RoomDoorController : MonoBehaviour
{
    [SerializeField] private GameObject go;
    [SerializeField] private Transform openDoorRef;
    [SerializeField] private Transform closedDoorRef;
    Vector3 _openPosition;
    Vector3 _closedPosition;
    public bool stayClosed = false;
    const float TimeToMove = 0.3f;


    void Start()
    {
        _openPosition = openDoorRef.position;
        _closedPosition = closedDoorRef.position;
        Open();
    }

    private void OnEnable() => GameManager.OnAfterStateChanged += OnStateChange;


    private void OnDisable() => GameManager.OnAfterStateChanged -= OnStateChange;

    
    
    void OnStateChange(GameState state)
    {
        if (state == GameState.InRoom) Close();
        else if (state == GameState.BetweenRooms) Open();
    }

    public void Open()
    {
        if (!stayClosed) UpdatePosition(_closedPosition, _openPosition);
    }

    public void Close() => UpdatePosition(_openPosition, _closedPosition);


    void UpdatePosition(Vector3 start, Vector3 end)
    {
        DOTween.To(
            () => go.transform.position,
            x => go.transform.position = x,
            endValue: end,
            TimeToMove
            );
    }
}