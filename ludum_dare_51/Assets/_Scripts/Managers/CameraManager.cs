using System;
using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    [SerializeField] public float dampTime = 0.15f;
    [SerializeField] bool targetSet = false;

    private GameEventManager gm;
    private Transform _target;
    private Camera _camera;
    private Vector3 _velocity = Vector3.zero;
    Vector3 _targetPos;
    Vector3 _camPos;

    private void Awake()
    {
        _camera = Camera.main;
        targetSet = false;
    }

    private void OnEnable()
    {
        GameEventManager.OnAfterStateChanged += OnStateChange;
    }

    private void OnDisable()
    {
        GameEventManager.OnAfterStateChanged -= OnStateChange;
    }

    private void OnStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.StartingGame:
                SetTargetToPlayer();
                break;
            case GameState.Lose:
                UnsetTarget();
                break;
        }
    }

    private void UnsetTarget()
    {
        _target = null;
        targetSet = false;
    }

    private void SetTargetToPlayer()
    {
        _target = PlayerManager.Instance.transform;
        Debug.Log("Camera target set: " + _target.name);
        targetSet = true;
    }

    void Update()
    {
        if (targetSet)
        {
            SlerpToTarget();
        }
    }


    void SlerpToTarget()
    {
        _targetPos = _target.position;
        _camPos = transform.position;

        float targetScreenZ = _camera.WorldToViewportPoint(_targetPos).z;
        Vector3 delta = _targetPos -
                        _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,
                            targetScreenZ));
        Vector3 destination = _camPos + delta;
        transform.position = Vector3.SmoothDamp(
            _camPos, destination,
            ref _velocity, dampTime);
    }
}