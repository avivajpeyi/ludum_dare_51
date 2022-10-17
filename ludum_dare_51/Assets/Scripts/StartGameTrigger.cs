using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameTrigger : MonoBehaviour
{
    
    [SerializeField] public Animator _crossfadeAnim;
    [SerializeField] public Animator _spriteFocusAnim;

    void StartGameTransition()
    {
        _crossfadeAnim.SetTrigger("start");
        _spriteFocusAnim.SetTrigger("start");
    }
    
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            StartGameTransition();
            // delay for 1 second
            Invoke("LoadGame", 1);
            
        }
    }

    void LoadGame()
    {
        SceneManager.LoadScene(1);
    }



}
