
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class basic_menu : MonoBehaviour
{
    
    public void PlayGame() {
        SceneManager.LoadScene("Other menu");
    }
	
    public void Quit() {
        Debug.Log("Quit!");
        Application.Quit();
    }


}


