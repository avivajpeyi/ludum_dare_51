using System;
using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
    
    private GameManager gm;
    private Transform _target;
    private Camera _camera;
    private CinemachineBrain _cineBrain;
    
    private Vector3 _velocity = Vector3.zero;
    Vector3 _targetPos;
    Vector3 _camPos;

    
    private CinemachineVirtualCamera ActiveVirtualCamera
    {
        get
        {
            return _cineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        }
    }
    
    
    
    private void Awake()
    {
        _cineBrain = GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        gm = GameManager.Instance;
        mainVirtualCamera.Follow = PlayerManager.Instance.transform;
        mainVirtualCamera.Priority = 100;
    }
    
    void PrioitizeMainVirtualCamera()=> mainVirtualCamera.Priority = 100;
    void DeprioritizeMainVirtualCamera()=> ActiveVirtualCamera.Priority = 0;
    
    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += OnStateChange;
        GameManager.OnPlayerTakeDamage += CameraShake;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= OnStateChange;
        GameManager.OnPlayerTakeDamage -= CameraShake;
    }

    private void OnStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.StartingGame:
                PrioitizeMainVirtualCamera();
                break;
            case GameState.BetweenRooms:
                PrioitizeMainVirtualCamera();
                break;
            case GameState.InRoom:
                DeprioritizeMainVirtualCamera();
                break;
        }
    }


    public void CameraShake()
    {
        ActiveVirtualCamera.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }

}