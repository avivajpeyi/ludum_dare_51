using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] private bool doorIsOpen = true;
    [SerializeField] private GameObject door;
    [SerializeField] private Transform openDoorRef;
    [SerializeField] private Transform closedDoorRef;
    Vector3 openPosition; 
    Vector3 closedPosition;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        openPosition = openDoorRef.position;
        closedPosition = closedDoorRef.position;
        Open();
    }
    

    
    
    public void Open()
    {
        doorIsOpen = true;
        StartCoroutine(LerpDoor(closedPosition, openPosition, 0.3f));
    }

    public void Close()
    {
        doorIsOpen = false;
        StartCoroutine(LerpDoor(openPosition, closedPosition, 0.3f));
    }
    
    
    IEnumerator LerpDoor(Vector3 startValue, Vector3 endValue, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            door.transform.position = Vector3.Lerp(startValue, endValue, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        door.transform.position = endValue;
    }
    
    
    
    
}