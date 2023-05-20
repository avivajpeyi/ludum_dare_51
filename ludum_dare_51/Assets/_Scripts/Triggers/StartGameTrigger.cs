using UnityEngine;

public class StartGameTrigger : MonoBehaviour
{
    [SerializeField] private Animator crossfadeAnim;
    [SerializeField] private Animator spriteFocusAnim;

    void StartGameTransition()
    {
        crossfadeAnim.SetTrigger("start");
        spriteFocusAnim.SetTrigger("start");
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            StartGameTransition();
            Invoke("LoadGame", 1);
        }
    }

    void LoadGame()
    {
        SceneManager.GoToGameScene();
    }
}