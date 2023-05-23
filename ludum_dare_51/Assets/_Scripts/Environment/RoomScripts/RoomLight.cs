using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RoomLight : MonoBehaviour
{
    private readonly float _initIntensity = 0.3f;
    private readonly float _maxIntensity = 0.55f;
    private readonly float _minIntensity = 0.1f;

    private Light2D _light;
    private RoomManager _room;
    
    void Awake()
    {
        _room = GetComponentInParent<RoomManager>();
        _light = GetComponent<Light2D>();
        _light.intensity = _initIntensity;
    }

    private void OnEnable() => RoomFactory.OnActivateRoom += OnRoomActivation;

    private void OnDisable() => RoomFactory.OnActivateRoom -= OnRoomActivation;


    void OnRoomActivation(RoomManager triggeredRoom)
    {
        if (triggeredRoom == _room) UpdateLightIntensity(_maxIntensity);
        else UpdateLightIntensity(_minIntensity);
    }

    void UpdateLightIntensity(float intensity)
    {
        DOTween.To(
            () => _light.intensity,
            x => _light.intensity = x,
            intensity, 1f
        );
    }
}