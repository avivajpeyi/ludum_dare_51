using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PickupTrigger : MonoBehaviour
{
    public GameObject pickupFx;
    
    private void OnTriggerEnter2D(Collider2D collider) {
        PlayerManager playerManager = collider.GetComponent<PlayerManager>();
        if (playerManager != null) {
            Debug.Log("Player collected z");
            Instantiate(pickupFx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
