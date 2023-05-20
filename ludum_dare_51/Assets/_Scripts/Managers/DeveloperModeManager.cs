using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class DebugCommand
{
    public string ID { get; protected set; }
    public string description { get; protected set; }
    public string format { get; protected set; }
    public Action command;
    

    public DebugCommand(string id, string description, string format, Action command)
    {
        ID = id;
        this.description = description;
        this.format = format;
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}





public class DeveloperModeManager : MonoBehaviour
{
    private bool showConsole;
    public List<object> commandList;
    string input = "";
    private GameEventManager _gameEventManager;
    private TimerManager _timerManager;


    public static DebugCommand GOD_MODE;
    public static DebugCommand SKIP_ROOM;
    
    

    
    public void OnToggleDebug()
    {
        showConsole = !showConsole;
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        foreach (DebugCommand command in commandList)
        {
            if (input == command.ID)
            {
                command.Invoke();
            }
        }
    }

    private void Awake()
    {
        commandList = new List<object>();
       
    }


    private void Start()
    {
        _gameEventManager = GameEventManager.Instance;
        _timerManager = TimerManager.Instance;
        InitCommands();
        
        
        #if UNITY_EDITOR
        showConsole = true;
        #endif
        
    }


    void InitCommands()
    {
        GOD_MODE = new DebugCommand("god", "Toggle god mode", "god", () =>
        {
            _gameEventManager.TriggerToggleGodMode();
        });
        SKIP_ROOM = new DebugCommand("skip", "Skip room", "skip",  () =>
        {
            _timerManager.SkipTimer();
        });
        commandList.Add(GOD_MODE);
        commandList.Add(SKIP_ROOM);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            OnReturn();
            OnToggleDebug();
        }
    }
    
    
    private void OnGUI()
    {
        if (!showConsole) return;
        String s = GameEventManager.Instance.State.ToString();
        
        float y = 0f;
        GUI.Box(new Rect(0,y,Screen.width,30), s);
        GUI.backgroundColor = Color.white;
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }
}
