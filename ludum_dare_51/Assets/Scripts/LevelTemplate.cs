using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    private Transform endRefTransform;
    private Transform startRefTransform;
    private Transform playerReference;
    private Transform deathZone;
    public LevelTemplate PrevTemplate;
    
    void Awake()
    {
        endRefTransform = transform.Find("EndPosition");
        startRefTransform = transform.Find("StartPosition");
        playerReference = transform.Find("PlayerReference");
        deathZone = transform.Find("DeathZone");
        endRefTransform.GetComponent<SpriteRenderer>().enabled = false;
        startRefTransform.GetComponent<SpriteRenderer>().enabled = false;
        deathZone.GetComponent<SpriteRenderer>().enabled = false;
        playerReference.gameObject.SetActive(false);
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
