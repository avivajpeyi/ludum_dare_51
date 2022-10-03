using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{

    private float moveSpeed = 30f;

    private Rigidbody2D rigidBody;

    private Player player;

    private Vector2 moveDirection;

    private Vector2 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<Player>();

        playerPos = (player.transform.position - transform.position).normalized;

        moveDirection = new Vector2 (playerPos.x, 0);

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
            player.Die();

        }
    }
}
