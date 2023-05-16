using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class pickupZ : MonoBehaviour
{
    public GameObject pickupFx;
    
    private void OnTriggerEnter2D(Collider2D collider) {
        Player player = collider.GetComponent<Player>();
        if (player != null) {
            // Player hit Spikes
            Debug.Log("Player collected z");
            Instantiate(pickupFx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
