using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    private Transform endRefTransform;
    private Transform startRefTransform;
    private Transform playerReference;
    
    void Awake()
    {
        endRefTransform = transform.Find("EndPosition");
        startRefTransform = transform.Find("StartPosition");
        playerReference = transform.Find("PlayerReference");
        // endRefTransform.GetComponent<SpriteRenderer>().enabled = false;
        // startRefTransform.GetComponent<SpriteRenderer>().enabled = false;
        playerReference.gameObject.SetActive(false);
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
        // https://answers.unity.com/questions/460064/align-parent-object-using-child-object-as-point-of.html
        playerReference.position = startRefTransform.position;
        // tr1P - Game Object 1 parent transform
        // tr1c - Game Object 1 child transform
        // tr2P - Game Object 2 parent transform
        // tr2C - Game Object 2 child transform
        Transform tr1P = transform;
        Transform tr1C = startRefTransform;
        Transform tr2P = previous.transform;
        Transform tr2C = previous.GetEndTransform();
        Vector3 v1 = tr1P.position - tr1C.position;
        Vector3 v2 = tr2C.position - tr2P.position;
        tr1P.position = tr2C.position + v2.normalized * v1.magnitude;
    }

    
}
