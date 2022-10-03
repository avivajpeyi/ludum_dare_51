using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private float moveSpeed = 30f;

    private Rigidbody2D rigidBody;

    private Player player;

    private Monster monster;

    private Vector2 moveDirection;

    private Vector2 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<Player>();
        //monster = GameObject.FindObjectOfType<Monster>(); // FIXME will this be a problem with multiple monsters?
        // as long as there is homogeneity in monsters shooting mode for each level, it should be OK?

        //if (monster.aimStraight) //shooting mode: shoot straight
        //{ 
        //    moveDirection = new Vector2 (-1, 0); // shoot left at all times
        //}
        //else //can make this into an elseif if we have more 'shooting modes'
        //{
        playerPos = (player.transform.position - transform.position).normalized;
        moveDirection = playerPos;
        //}

        rigidBody.velocity = moveDirection * moveSpeed;

        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            // Player hit laser
            Debug.Log("Player hit with laser ---> ded");
            Destroy(gameObject);
            player.Die();
        }
    }
}
