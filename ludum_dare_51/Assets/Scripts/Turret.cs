using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Turret : MonoBehaviour
{
    public GameObject projectile;
    public float turretRotationSpeed = 20f;
    
    public float minRotationoAngle = -90f;
    public float maxRotationAngle = 90f;
    private GameObject player;

    public float projectileSpeed = 30f;
    public float randomDelay = 0f;
    public float fireRate = 4f;
    float nextFire;
    private bool isOn;
    
    private GameEventManager _manager;

    public bool aimAtPlayer = true;


    private void Awake()
    {        
        player = GameObject.FindGameObjectWithTag("Player");
        _manager = FindObjectOfType<GameEventManager>();
        nextFire = Time.time + Random.Range(0, randomDelay);
        isOn = true;
    }

    private void UpdateTimeForNextShot()
    {
        nextFire = Time.time + Random.Range(0, randomDelay) + fireRate;
    }
    
    private void OnEnable()
    {
        if (_manager == null) return;
        _manager.OnStartLevel += turnOn;
        _manager.OnEndLevel += turnOff;
    }

    private void OnDisable()
    {
        if (_manager == null) return;
        _manager.OnStartLevel -= turnOn;
        _manager.OnEndLevel -= turnOff;
    }

    
    public void turnOn() {isOn = true; }

    public void turnOff() {isOn = false; }


    public void RotateTowardsPlayer()
    {
        if (player==null) return;
        // //rotate the 
        // Vector3 targ = player.transform.position;
        // targ.z = 0f;
        // Vector3 objectPos = transform.position;
        // targ.x -= objectPos.x;
        // targ.y -= objectPos.y;
        //
        // float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        // angle = Mathf.Clamp(angle, minRotationoAngle, maxRotationAngle);
        //
        // transform.rotation = Quaternion.RotateTowards(
        //     transform.rotation,
        //     Quaternion.Euler(new Vector3(0, 0, angle)),
        //     20f);
        //
        // rotate towards player
        // float strength = 0.5f;
        // Quaternion targetRotation = Quaternion.LookRotation (
        //     player.transform.position - transform.position);
        // float str = Mathf.Min (strength * Time.deltaTime, 1);
        // transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);

        Vector3 t = player.transform.position;
        float SPEED = 4f;
        float angle = Mathf.Atan2(t.y - transform.position.y, t.x -transform.position.x ) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, SPEED * Time.deltaTime);
        
        
    }

    void Shoot()
    {
        // see https://stackoverflow.com/questions/59026221/unity3d-help-shooting-object-in-direction-of-player-rotation
        GameObject shotLaser = Instantiate(projectile, transform.position, transform.rotation);
        shotLaser.GetComponent<Projectile>().moveSpeed = projectileSpeed;
        UpdateTimeForNextShot();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isOn) return;
   
        
        if (aimAtPlayer) RotateTowardsPlayer();
  

        // check if time to fire
        if (Time.time > nextFire)
        {
            Shoot();
        }
    }




}
