using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Starts/ends game 
public class GameEventManager : MonoBehaviour
{

    
    public static GameEventManager instance;
    public delegate void StartGame(); 
    public  event StartGame OnStartGame;
    
    public delegate void EndGame(); 
    public  event EndGame OnEndGame;
    
    public delegate void StartLevel(); 
    public  event StartLevel OnStartLevel;
    
    public delegate void EndLevel(); 
    public  event EndLevel OnEndLevel;

    bool waitingToStart = true;
    public bool isGameOver = false;
    

    private void Update()
    {
        if (Input.anyKeyDown && waitingToStart)
        {
            RunStartGame();
        }
        if (isGameOver && Input.anyKeyDown)
        {
            RestartGame();
        }
    }

    /**
     * Starts the game:
     * called if any key pressed or if the player clicks the start button
     */
    public void RunStartGame() 
    {
        waitingToStart = false;
        if (OnStartGame != null)
        {
            Debug.Log("Start game");
            if (!isGameOver) OnStartGame();
        }
    }
    
    /**
     * Ends the game:
     * called when the player dies (see Player.Die)
     */
    public void RunEndGame()
    {
        isGameOver = true;
        if (OnEndGame != null)
        {
            OnEndGame();
        }
    }
    
    
    /**
     * Starts a level:
     * called when the player enters a new room (see LevelStartTrigger)
     */
    public  void RunStartLevel()
    {
        if (OnStartLevel != null)
        {
            Debug.Log("Start level");
            if (!isGameOver) OnStartLevel();
        }
    }
    
    /**
     * Ends a level:
     * called when the level timer finishes (see LevelTimer)
     */
    public void RunEndLevel()
    {
        if (OnEndLevel != null)
        {
            Debug.Log("end level");
            if (!isGameOver) OnEndLevel();
        }
    }
    
    /**
     * Restarts the game:
     * called if any key pressed or the player clicks the restart button
     */
    void RestartGame()
    {
        isGameOver = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
    
}

