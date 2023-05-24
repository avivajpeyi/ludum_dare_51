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
    private GameManager _gm;
    private TimerManager _timerManager;
    private RoomFactory _roomFactory;


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
        _gm = GameManager.Instance;
        _roomFactory = RoomFactory.Instance;
        _timerManager = TimerManager.Instance;
        InitCommands();


#if UNITY_EDITOR
        showConsole = true;
#endif
    }


    void InitCommands()
    {
        GOD_MODE = new DebugCommand("god", "Toggle god mode", "god",
            () => { _gm.TriggerToggleGodMode(); });
        SKIP_ROOM = new DebugCommand("skip", "Skip room", "skip",
            () => { _timerManager.SkipTimer(); });
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


    private int _fps;

    private int FPS
    {
        get
        {
            if (Time.frameCount % 60 == 0)
            {
                _fps = (int)(1.0f / Time.smoothDeltaTime);
                _fps -= _fps % 10;
            }
            return _fps;
        }
    }
    
    String RoomName
    {
        get
        {
            try {if (_gm.State == GameState.InRoom) return RoomFactory.ActiveRoomName; }
            catch (Exception e) {return "ERROR"; }
            return "No Room";
        }
    }

    void AddDeveloperInfo()
    {
        String state = GameManager.Instance.State.ToString();
        String s = $"FPS: {FPS:000} | State: {state} | Room: {RoomName}";
        // Set box transparent with no border
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.Box(new Rect(0, Screen.height - 20, Screen.width, 20), s);
    }


    private void OnGUI()
    {
        if (!Application.isPlaying) return;
        #if UNITY_EDITOR
        AddDeveloperInfo();
        #endif
        
        
        if (!showConsole) return;
        AddDeveloperInfo();
        float y = 0f;
        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = Color.white;
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }
}