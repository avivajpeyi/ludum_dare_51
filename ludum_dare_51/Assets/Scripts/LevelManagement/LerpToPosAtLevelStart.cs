using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToPosAtLevelStart : MonoBehaviour
{

    [SerializeField] private GameObject go;
    [SerializeField] private Transform openDoorRef;
    [SerializeField] private Transform closedDoorRef;
    Vector3 openPosition; 
    Vector3 closedPosition;
    public bool stayClosed = false;
    [SerializeField] private bool isOpen=false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        openPosition = openDoorRef.position;
        closedPosition = closedDoorRef.position;
        Open();
    }
    
    
    public void Open()
    {
        if (!stayClosed)
        {
            StartCoroutine(LerpPosition(closedPosition, openPosition, 0.3f));
            isOpen = true;
        }

        
        
    }

    public void Close()
    {
        StartCoroutine(LerpPosition(openPosition, closedPosition, 0.3f));
        isOpen = false;
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
