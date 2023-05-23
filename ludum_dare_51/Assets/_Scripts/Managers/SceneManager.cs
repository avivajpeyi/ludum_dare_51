using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    GameManager _gm;

    private const string MainMenuSceneName = "StartScene";
    private const string GameSceneName = "MainGameScene";


    string SceneName
    {
        get
        {
            Scene s = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            return s.name;
        }
    }

    private void Start()
    {
        _gm = GameManager.Instance;
        Debug.Log($"SceneManager: Start: SceneName: {SceneName}");
        if (SceneName == MainMenuSceneName)
        {
            _gm.ChangeState(GameState.MainMenu);
        }
        else if (SceneName == GameSceneName)
        {
            _gm.ChangeState(GameState.StartingGame);
        }
    }


    public static void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenuSceneName);
    }

    public static void GoToGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameSceneName);
    }
}