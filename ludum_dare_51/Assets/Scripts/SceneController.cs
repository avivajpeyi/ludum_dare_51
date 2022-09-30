using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Update()
    {
        // if 'N' is pressed, load the next scene
        if (Input.GetKeyDown(KeyCode.N))
        {
            GoToNextScene();
        }
    }

    // Go to next scene
    public void GoToNextScene()
    {
        // Get current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneId = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        // Load next scene
        SceneManager.LoadScene(sceneId);
    }
}
