using System;
using UnityEngine;
using System.Collections;
 
public class SmoothCamera : MonoBehaviour {
     
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    private Camera _camera;
    private GameObject p;

    private void Awake()
    {
        _camera = Camera.main;
        p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            target = p.transform;
    }

    // Update is called once per frame
    void Update () 
    {
        if (target)
        {
            Vector3 point = _camera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
     
    }
}