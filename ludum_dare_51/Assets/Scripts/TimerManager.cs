using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {

    public static int timeRemaining = 0;

    private void OnTriggerEnter2D(Collider2D collider) {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            ResetTimer();
            HourglassWindow.ResetTimer();
            Destroy(gameObject);
        }
    }

    public static void ResetTimer() {
        timeRemaining = 10;
    }
    

}
