using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelRoom : MonoBehaviour
{
    
    public LevelRoom prevRoom;
    private Transform endRefTransform;
    private Transform startRefTransform;
    private Transform playerReference;
    private Transform deathZone;
    private Light2D roomLight;


    private GameObject timer;
    [SerializeField] private LevelDoor startDoor;
    [SerializeField] private LevelDoor endDoor;
    private GameEventManager _gameEventManager;
    
    
    void Awake()
    {
        SetReferences();
        endRefTransform.GetComponent<SpriteRenderer>().enabled = false;
        startRefTransform.GetComponent<SpriteRenderer>().enabled = false;
        deathZone.GetComponent<SpriteRenderer>().enabled = false;
        playerReference.gameObject.SetActive(false);
        
    }
    
    void SetReferences()
    {
        _gameEventManager = FindObjectOfType<GameEventManager>();
        endRefTransform = transform.Find("EndPosition");
        endDoor = endRefTransform.GetComponentInChildren<LevelDoor>();
        startRefTransform = transform.Find("StartPosition");
        startDoor = startRefTransform.GetComponentInChildren<LevelDoor>();
        playerReference = transform.Find("PlayerReference");
        deathZone = transform.Find("DeathZone");
        roomLight = transform.Find("RoomLight").GetComponent<Light2D>();
        
        List<Transform>  requiredTrans = new List<Transform>{
            endRefTransform, startRefTransform, playerReference, deathZone, roomLight.transform
        };
        foreach (Transform t in requiredTrans)
        {
            if (t == null)
            {
                Debug.LogError("Missing required transform in " + name);
            }
        }
    }

    private void OnEnable()
    {
        _gameEventManager.OnStartLevel += CloseLevelDoors;
        _gameEventManager.OnEndLevel += OpenExitDoor;
    }

    private void OnDisable()
    {
        _gameEventManager.OnStartLevel -= CloseLevelDoors;
        _gameEventManager.OnEndLevel -= OpenExitDoor;
    }


    public void CloseLevelDoors()
    {
        startDoor.Close();
        endDoor.Close();
    }
    
    public void OpenExitDoor()
    {
        endDoor.Open();
        startDoor.Open();
        Debug.Log("Open " + name + " ExitDoor" );
    }

    private void Start()
    {
        if (prevRoom!=null)
            AlignToPreviousLevelPart(prevRoom);
        roomLight.intensity = 0.35f;
    }

    private void OnDrawGizmos()
    {
        // To help debug
        // Turn on in the editor 
        // https://medium.com/geekculture/how-to-do-visual-debugging-editing-using-gizmos-in-unity-c-e3b8ea711b30
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            BoxCollider2D boxCollider2D = deathZone.GetComponent<BoxCollider2D>();
            Gizmos.DrawCube(boxCollider2D.bounds.center, boxCollider2D.bounds.size);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(GetEndPoint(), 2f);
            Gizmos.DrawSphere(GetStartPoint(), 2f);
        }
    }
    
    public Transform GetStartTransform()
    {
        return startRefTransform;
    }
    
    public Vector3 GetStartPoint()
    {
        return startRefTransform.position;
    }
    
    public Transform GetEndTransform()
    {
        return endRefTransform;
    }
    
    public Vector3 GetEndPoint()
    {
        return endRefTransform.position;
    }

    public void AlignToPreviousLevelPart(LevelRoom previous)
    {
        Transform curStart = startRefTransform;
        Transform prevEnd = previous.GetEndTransform();
        transform.position = prevEnd.position - curStart.localPosition;
    }

    
}
