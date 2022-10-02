using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverWindow : MonoBehaviour {

    private static GameOverWindow instance;
    private static bool canRestart = false;
    
    private void Awake() {
        instance = this;
        // transform.Find("retryBtn").GetComponent<Button>().onClick = Retry;
        instance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (canRestart && Input.anyKeyDown) Retry();
    }

    public void Retry() {
        TimerManager.ResetTimer();
        SceneManager.LoadScene(0);
    }

    public static void Show() {
        instance.gameObject.SetActive(true);
        canRestart = true;
    }

}
