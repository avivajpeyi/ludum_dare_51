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

    public AudioClip shootSound;

    public Transform muzzleTip;


    public float randomDelay = 0f;
    public float fireRate = 4f;
    float nextFire;
    public bool isOn;

    private GameEventManager _manager;

    public bool aimAtPlayer = true;

    private bool playerInSight = false;

    private float playerCheckDistance = 35f;


    public LayerMask blockingLayers;


    private void Awake()
    {
        nextFire = Time.time + Random.Range(0, randomDelay);
        startAngle = GetAngleToPosition(transform.up);

        float bound1 = startAngle + 90;
        float bound2 = startAngle - 90;
        minAngle = Mathf.Min(bound1, bound2);
        maxAngle = Mathf.Max(bound1, bound2);
        targetAngle = startAngle;
    }

    private void OnEnable()
    {
        GameEventManager.OnAfterStateChanged += OnStateChange;
    }

    private void OnDisable()
    {
        GameEventManager.OnAfterStateChanged += OnStateChange;
    }


    void OnStateChange(GameState state)
    {
        isOn = state == GameState.InRoom;
    }


    private void Start()
    {
        player = PlayerManager.Instance.gameObject;
    }


    Vector3 makeLineFromAngle(float angle)
    {
        return transform.position + Quaternion.Euler(0, 0, angle) * Vector3.up * 5;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        // draw a line along the angle
        Gizmos.DrawLine(transform.position, makeLineFromAngle(startAngle));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(makeLineFromAngle(minAngle), makeLineFromAngle(maxAngle));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, makeLineFromAngle(targetAngle));

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, playerCheckDistance);
        if (player != null)
        {
            Gizmos.DrawRay(
                transform.position,
                (player.transform.position - transform.position).normalized *
                playerCheckDistance);
        }
    }


    private void UpdateTimeForNextShot()
    {
        nextFire = Time.time + Random.Range(0, randomDelay) + fireRate;
    }


    float GetAngleToPosition(Vector2 p)
    {
        float a = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;
        return a - offsetAngle;
    }


    void CheckIfPlayerInSight()
    {
        playerInSight = false;


        // raycast to check if player is in sight
        Vector2 pos = transform.position;
        Vector2 dir = (Vector2)player.transform.position - pos;
        RaycastHit2D hit =
            Physics2D.Raycast(pos, dir, playerCheckDistance, blockingLayers);
        Debug.DrawRay(pos, dir * playerCheckDistance, Color.cyan);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerInSight = true;
            }
        }
    }


    public void RotateTowardsPlayer()
    {
        if (player == null) return;
        targetAngle = GetAngleToPosition(player.transform.position - transform.position);

        if (Mathf.Abs(targetAngle - startAngle) > 90f)
        {
            targetAngle = startAngle;
        }


        // it would be cool if we can get the targetAngle to be the max/min angle if it's outside the range
        // but I don't know how to do that yet
        // why do you want to do that? Looks kinda weird...
        // this worked: targetAngle = minAngle; (assuming this is what you meant) -AP

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        // see https://stackoverflow.com/questions/59026221/unity3d-help-shooting-object-in-direction-of-player-rotation
        GameObject shotLaser =
            Instantiate(projectile, muzzleTip.position, transform.rotation);
        shotLaser.GetComponent<Projectile>().moveSpeed = projectileSpeed;
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
        UpdateTimeForNextShot();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn) return;

        if (aimAtPlayer)
        {
            CheckIfPlayerInSight();
            RotateTowardsPlayer();
            if (!playerInSight) return;
        }

        // check if time to fire
        if (Time.time > nextFire)
        {
            Shoot();
        }
    }
}