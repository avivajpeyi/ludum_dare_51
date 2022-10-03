using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject laser;

    float fireRate;
    float nextFire;
    bool monsterAlive;

    // Start is called before the first frame update
    void Start()
    {
        monsterAlive = true;
        fireRate = 4f;
        nextFire = Time.time;
    }

    public void turnOnMonster()
    {
        monsterAlive = true;
    }

    public void turnOffMonster()
    {
        monsterAlive = false;
    }

    // Update is called once per frame
    void Update()
    {
        // check if time to fire
        if (monsterAlive && Time.time > nextFire)
        {
            Instantiate(laser, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
    }

}
