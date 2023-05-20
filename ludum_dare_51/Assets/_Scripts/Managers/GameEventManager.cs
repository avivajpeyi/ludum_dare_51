using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Starts/ends game 
public class GameEventManager : Singleton<GameEventManager>
{

    
    public bool isGameOver = false;

    private void Update()
    {
        if (isGameOver && Input.anyKeyDown)
        {
            RestartGame();
        }
    }



    void RestartGame()
    {
        isGameOver = false;
        SceneManager.GoToGameScene();
    }

    
    public delegate void PlayerTakeDamage();

    public static event PlayerTakeDamage OnPlayerTakeDamage;
    public void TriggerTakeDamage() => OnPlayerTakeDamage?.Invoke();

    public delegate void ToggleGodMode();

    public static event ToggleGodMode OnToggleGodMode;
    public void TriggerToggleGodMode() => OnToggleGodMode?.Invoke();

    #region StateHandling

    

    
    public static event Action<GameState, GameState> OnBeforeStateChanged;

    public static event Action<GameState> OnAfterStateChanged;


    private GameState _state = GameState.MainMenu;

    public GameState State
    {
        get { return _state; }

        private set
        {
            if (_state != value)
            {
                _state = value;
            }
            else
            {
                Debug.LogWarning(
                    $"State {value} changed to the same state"
                );
            }
        }
    }
    

    public void ChangeState(GameState newState)
    {
        Debug.Log($"<color=green>ChangeState: {State}-->{newState}</color>");
        OnBeforeStateChanged?.Invoke(State, newState);

        State = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.StartingGame:
                HandleStartingGame();
                break;
            case GameState.BetweenRooms:
                HandleBetweenRooms();
                break;
            case GameState.InRoom:
                HandleInRoom();
                break;
            case GameState.Paused:
                HandlePause();
                break;
            case GameState.Win:
                HandleWin();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
    }

    private void HandleMainMenu()
    {
        Debug.Log("Handle main menu");
        Debug.Assert(State == GameState.MainMenu);
    }


    private void HandleStartingGame()
    {
        Debug.Log("Generating generating level");
        Debug.Assert(State == GameState.StartingGame);
        isGameOver = false;
    }


    private void HandleBetweenRooms()
    {
        Debug.Assert(State == GameState.BetweenRooms);
    }

    private void HandleInRoom()
    {
        Debug.Assert(State == GameState.InRoom);
    }


    private void HandleWin()
    {
        Debug.Assert(State == GameState.Win);
    }

    private void HandleLose()
    {
        Debug.Assert(State == GameState.Lose);
        isGameOver = true;
    }

    private void HandlePause()
    {
        Debug.Assert(State == GameState.Paused);
        isGameOver = true;
    }
    
    
    
    #endregion
    
}


[Serializable]
public enum GameState
{
    MainMenu = 0,
    StartingGame = 1,
    InRoom = 2,
    BetweenRooms = 3,
    Paused = 4,
    Win = 5,
    Lose = 6,
}