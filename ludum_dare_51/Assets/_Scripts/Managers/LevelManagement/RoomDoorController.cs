using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoorController : MonoBehaviour
{

    [SerializeField] private GameObject go;
    [SerializeField] private Transform openDoorRef;
    [SerializeField] private Transform closedDoorRef;
    Vector3 _openPosition; 
    Vector3 _closedPosition;
    public bool stayClosed = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _openPosition = openDoorRef.position;
        _closedPosition = closedDoorRef.position;
        Open();
    }
    
    
    public void Open()
    {
        if (!stayClosed)
        {
            StartCoroutine(LerpPosition(_closedPosition, _openPosition, 0.3f));
        }

        
        
    }

    public void Close()
    {
        StartCoroutine(LerpPosition(_openPosition, _closedPosition, 0.3f));
    }
    
    
    IEnumerator LerpPosition(Vector3 startValue, Vector3 endValue, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            go.transform.position = Vector3.Lerp(startValue, endValue, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        go.transform.position = endValue;
    }
    
    
}
