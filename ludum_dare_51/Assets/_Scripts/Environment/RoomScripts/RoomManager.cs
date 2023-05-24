using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class RoomManager : MonoBehaviour
{
    private RoomManager _prevRoom;
    private Transform _endDoorTransform;
    private Transform _startDoorTransform;
    private Transform _playerReference;
    private Transform _deathZone;
    private List<Item> _items;

    private void OnEnable()
    {
        RoomFactory.OnFinishRoom += RoomFinished;
    }
    
    private void OnDisable()
    {
        RoomFactory.OnFinishRoom -= RoomFinished;
    }


    private void RoomFinished(RoomManager r)
    {
        if (r!=this) return;
        if (!AllItemsCollected)
        {
            GameManager.Instance.TriggerTakeDamage();
            Debug.Log("Not all items have been collected");
        }

    }


    void Awake()
    {
        SetReferences();
        _endDoorTransform.GetComponent<SpriteRenderer>().enabled = false;
        _startDoorTransform.GetComponent<SpriteRenderer>().enabled = false;
        _deathZone.GetComponent<SpriteRenderer>().enabled = false;
        _playerReference.gameObject.SetActive(false);
    }

    void SetReferences()
    {
        _endDoorTransform = transform.Find("EndPosition");
        _startDoorTransform = transform.Find("StartPosition");
        _playerReference = transform.Find("PlayerReference");
        _deathZone = transform.Find("DeathZone");
        _items = transform.GetComponentsInChildren<Item>().ToList();
        List<Transform> requiredTrans = new List<Transform>
        {
            _endDoorTransform, _startDoorTransform, _playerReference, _deathZone,
        };
        if (!Helpers.AllTransformsExist(requiredTrans))
        {
            Debug.LogError(name + "missing assigment:" + requiredTrans);
        }
    }

    private void Start()
    {
        if (_prevRoom != null) AlignToPreviousRoom(_prevRoom);
    }


    public Vector3 EndPoint => _endDoorTransform.position;
    public Transform EndTransform => _endDoorTransform;

    public void AlignToPreviousRoom(RoomManager previous)
    {
        Transform curStart = _startDoorTransform;
        Transform prevEnd = previous.EndTransform;
        _prevRoom = previous;
        transform.position = prevEnd.position - curStart.localPosition;
    }

    public bool AllItemsCollected => CurrentItemCount == RequiredItemCount;

    int RequiredItemCount => _items.Count;
    int CurrentItemCount => _items.Count(t => t.isCollected);
}