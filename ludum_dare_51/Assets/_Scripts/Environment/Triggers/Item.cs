using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    
    // Item Event
    public delegate void ItemCollected();
    public  event ItemCollected OnItemCollected;
    
    
    public GameObject pickupFx;
    public bool isCollected;

    

    private void OnTriggerEnter2D(Collider2D collider) {
        PlayerManager playerManager = collider.GetComponent<PlayerManager>();
        if (playerManager != null) {
            Debug.Log("Player collected item");
            Instantiate(pickupFx, transform.position, Quaternion.identity);
            isCollected = true;
            OnItemCollected?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
