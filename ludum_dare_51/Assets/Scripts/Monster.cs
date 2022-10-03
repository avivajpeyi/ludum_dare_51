using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject laser;

    private Player player;

    public float fireRate;
    float nextFire;
    bool monsterAlive;

    public bool aimAtPlayer = true;
    public bool aimStraight = false;

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
            player = GameObject.FindObjectOfType<Player>();
            if (player != null)
            {
                //FIXME
                //lines 50-60 should be in aimAtPlayer part of if statement
                // but if statement not functioning properly for some reason
                // both pieces of code work fine on their own

                //rotate the monster
                Vector3 targ = player.transform.position;
                targ.z = 0f;
                Vector3 objectPos = transform.position;
                targ.x = targ.x - objectPos.x;
                targ.y = targ.y - objectPos.y;

                float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
                
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                // instantiate laser with the above rotation
                Instantiate(laser, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

                if (aimAtPlayer == true)  
                {
                    // lines 50-60
                }
                else if (aimStraight == true)
                {
                    // instantiate laser with the above rotation
                    //Instantiate(laser, transform.position, Quaternion.identity);
                }
            }

            nextFire = Time.time + fireRate;
        }
    }

}
