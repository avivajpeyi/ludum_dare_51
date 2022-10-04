using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Turret : MonoBehaviour
{
    public float projectileSpeed = 30f;
    public GameObject projectile;
    public float rotationSpeed = 20f;
    
    [SerializeField] private float targetAngle = 0;
    public float offsetAngle = 90f;
    [SerializeField] private float startAngle;
    private GameObject player;
    private float minAngle = 0;
    private float maxAngle = 0;
    

   
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
        
        startAngle = GetAngleToPosition(transform.up);

        float bound1 = startAngle + 90;
        float bound2 = startAngle - 90;
        minAngle = Mathf.Min(bound1, bound2);
        maxAngle = Mathf.Max(bound1, bound2);
        targetAngle = startAngle;
    }
    
    
    Vector3 makeLineFromAngle(float angle)
    {
        return transform.position + Quaternion.Euler(0, 0, angle) * Vector3.up * 5;
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        // draw a line along the angle
        Gizmos.DrawLine(transform.position, makeLineFromAngle(startAngle));
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(makeLineFromAngle(minAngle), makeLineFromAngle(maxAngle));
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, makeLineFromAngle(targetAngle));
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

    
    float GetAngleToPosition(Vector2 p )
    {
        float a = Mathf.Atan2(p.y, p.x ) * Mathf.Rad2Deg;
        return a - offsetAngle;
    }

   
    
    public void RotateTowardsPlayer()
    {
        if (player==null) return;
        targetAngle = GetAngleToPosition(player.transform.position - transform.position);
        
        if (Mathf.Abs(targetAngle - startAngle) > 90f)
            targetAngle = startAngle;
        // it would be cool if we can get the targetAngle to be the max/min angle if it's outside the range
        // but I don't know how to do that yet

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
        targetRotation, rotationSpeed * Time.deltaTime);
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
            // for rotate tp player, only shoot if player in sight
        }
    }




}
