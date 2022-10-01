using System;
using UnityEngine;
using System.Collections;
 
public class SmoothCamera : MonoBehaviour {
     
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    private Camera camera;
    private Player p;

    private void Awake()
    {
        camera = Camera.main;
        p = FindObjectOfType<Player>();
        if (p != null)
            target = p.transform;
    }

    // Update is called once per frame
    void Update () 
    {
        if (target)
        {
            Vector3 point = camera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
     
    }
}