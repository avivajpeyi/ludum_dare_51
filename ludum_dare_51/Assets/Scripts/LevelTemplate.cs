using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    private Transform endRefTransform;
    private Transform startRefTransform;
    private Transform playerReference;

    public LevelTemplate PrevTemplate;
    
    void Awake()
    {
        endRefTransform = transform.Find("EndPosition");
        startRefTransform = transform.Find("StartPosition");
        playerReference = transform.Find("PlayerReference");
        // endRefTransform.GetComponent<SpriteRenderer>().enabled = false;
        // startRefTransform.GetComponent<SpriteRenderer>().enabled = false;
        playerReference.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (PrevTemplate!=null)
            AlignToPreviousLevelPart(PrevTemplate);
    }

    private void Update()
    {
        Vector2 ep = GetEndPoint();
        Vector2 sp = GetStartPoint();
        Debug.DrawLine(ep, ep + (Vector2.up*2), Color.blue);
        Debug.DrawLine(sp, sp + (Vector2.up*2), Color.blue);
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
