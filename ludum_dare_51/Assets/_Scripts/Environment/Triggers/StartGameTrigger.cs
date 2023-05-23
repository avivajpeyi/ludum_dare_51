using System;
using UnityEngine;

public class StartGameTrigger : MonoBehaviour
{
    [SerializeField] private Animator crossfadeAnim;
    [SerializeField] private Animator spriteFocusAnim;

    private bool animatorsAssigned = false;

    private void Start()
    {
        if (crossfadeAnim!=null && spriteFocusAnim!=null)
        {
            animatorsAssigned = true;
        }
    }

    void StartGameTransition()
    {
        crossfadeAnim.SetTrigger("start");
        spriteFocusAnim.SetTrigger("start");
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            if (animatorsAssigned) StartGameTransition();
            Invoke("LoadGame", 1);
        }
    }

    void LoadGame()
    {
        SceneManager.GoToGameScene();
    }
}