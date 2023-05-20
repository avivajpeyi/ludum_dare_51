using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider) {
        PlayerManager playerManager = collider.GetComponent<PlayerManager>();
        if (playerManager != null) {
            Debug.Log("Player hit Spikes --> ded");
            playerManager.Die();
            
        }
    }
}
