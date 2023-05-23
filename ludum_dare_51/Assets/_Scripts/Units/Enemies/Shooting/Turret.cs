using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Turret : MonoBehaviour
{
    
    RoomManager _myRoom;
    
    public float projectileSpeed = 30f;
    public GameObject projectile;
    public float rotationSpeed = 20f;

    [SerializeField] private float targetAngle = 0;
    public float offsetAngle = 90f;
    [SerializeField] private float startAngle;
    private GameObject _player;
    private float _minAngle = 0;
    private float _maxAngle = 0;

    public AudioClip shootSound;
    public Transform muzzleTip;


    public float randomDelay = 0f;
    public float fireRate = 4f;
    float _nextFire;
    public bool isOn;
    
    public bool aimAtPlayer = true;

    private bool _playerInSight = false;
    private readonly float _checkDist = 35f;


    public LayerMask blockingLayers;


    private void Awake()
    {
        _nextFire = Time.time + Random.Range(0, randomDelay);
        startAngle = GetAngleToPosition(transform.up);

        float bound1 = startAngle + 90;
        float bound2 = startAngle - 90;
        _minAngle = Mathf.Min(bound1, bound2);
        _maxAngle = Mathf.Max(bound1, bound2);
        targetAngle = startAngle;
    }
    
    private void Start()
    {
        _player = PlayerManager.Instance.gameObject;
        _myRoom = transform.parent.parent.GetComponent<RoomManager>();
        if (_myRoom == null) throw new Exception("Turret must be a child of a RoomManager");
    }

    void Update()
    {
        if (!isOn) return;
        if (aimAtPlayer)
        {
            CheckIfPlayerInSight();
            RotateTowardsPlayer();
            if (!_playerInSight) return;
        }
        if (Time.time > _nextFire) Shoot();
    }

    
    private void OnEnable()
    {
        RoomFactory.OnActivateRoom += OnActivateRoom;
        RoomFactory.OnFinishRoom += OnFinishRoom;
    }

    private void OnDisable()
    {
        RoomFactory.OnActivateRoom -= OnActivateRoom;
        RoomFactory.OnFinishRoom -= OnFinishRoom;
    }
    
    void OnFinishRoom(RoomManager r)=> isOn = false;

    void OnActivateRoom(RoomManager r) => isOn = r == _myRoom;





    Vector3 MakeLineFromAngle(float angle)
    {
        return transform.position + Quaternion.Euler(0, 0, angle) * Vector3.up * 5;
    }



    private void UpdateTimeForNextShot()
    {
        _nextFire = Time.time + Random.Range(0, randomDelay) + fireRate;
    }


    float GetAngleToPosition(Vector2 p)
    {
        float a = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;
        return a - offsetAngle;
    }


    void CheckIfPlayerInSight()
    {
        _playerInSight = false;
        Vector2 pos = transform.position;
        Vector2 dir = (Vector2)_player.transform.position - pos;
        RaycastHit2D hit =
            Physics2D.Raycast(pos, dir, _checkDist, blockingLayers);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            _playerInSight = true;
    }


    public void RotateTowardsPlayer()
    {
        targetAngle = GetAngleToPosition(_player.transform.position - transform.position);

        if (Mathf.Abs(targetAngle - startAngle) > 90f)
            targetAngle = startAngle;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        GameObject shotLaser =  Instantiate(projectile, muzzleTip.position, transform.rotation);
        shotLaser.GetComponent<Projectile>().moveSpeed = projectileSpeed;
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
        UpdateTimeForNextShot();
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        // draw a line along the angle
        Gizmos.DrawLine(transform.position, MakeLineFromAngle(startAngle));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(MakeLineFromAngle(_minAngle), MakeLineFromAngle(_maxAngle));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, MakeLineFromAngle(targetAngle));

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _checkDist);
        if (_player != null)
        {
            Gizmos.DrawRay(
                transform.position,
                (_player.transform.position - transform.position).normalized *
                _checkDist);
        }
    }

}