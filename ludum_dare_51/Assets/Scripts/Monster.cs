using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject laser;

    float fireRate;
    float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        fireRate = 4f;
        nextFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // check if time to fire
        if (Time.time > nextFire)
        {
            Instantiate(laser, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
    }

}
