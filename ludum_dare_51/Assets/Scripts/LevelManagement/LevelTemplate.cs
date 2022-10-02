using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    
    public LevelTemplate PrevTemplate;
    private Transform endRefTransform;
    private Transform startRefTransform;
    private Transform playerReference;
    private Transform deathZone;
    private Transform background;


    private GameObject timer;
    private LevelDoor[] doors;
    
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
        endRefTransform = transform.Find("EndPosition");
        startRefTransform = transform.Find("StartPosition");
        playerReference = transform.Find("PlayerReference");
        deathZone = transform.Find("DeathZone");
        background = transform.Find("Background");
        doors = transform.GetComponentsInChildren<LevelDoor>();
        List<Transform>  requiredTrans = new List<Transform>{
            endRefTransform, startRefTransform, playerReference,
            deathZone, background
        };
        foreach (Transform t in requiredTrans)
        {
            if (t == null)
            {
                Debug.LogError("Missing required transform in " + name);
            }
        }
    }

    public void StartLevel()
    {
        foreach (LevelDoor door in doors)
        {
            door.Close();
        }
        // start 10 second timer
        // end level after 10 second timer
        Debug.Log("Start level timer");
    }
    
    public void EndLevel()
    {
        foreach (LevelDoor door in doors)
        {
            door.Open();
        }
    }

    private void Start()
    {
        if (PrevTemplate!=null)
            AlignToPreviousLevelPart(PrevTemplate);
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

    public void AlignToPreviousLevelPart(LevelTemplate previous)
    {
        Transform curStart = startRefTransform;
        Transform prevEnd = previous.GetEndTransform();
        transform.position = prevEnd.position - curStart.localPosition;
    }

    
}
