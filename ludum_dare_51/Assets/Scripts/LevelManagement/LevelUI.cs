using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LevelUI : MonoBehaviour
{
   public GameObject[] levelUI;

   public TMP_Text timeTxt; 
   
   private void SetUiState(bool state)
   {
      foreach (GameObject ui in levelUI)
      {
         ui.SetActive(state);
      }
   }


   private void Awake()
   {
      HideUI();
   }
   
   public void ShowUI()
   {
      SetUiState(true);
   }
   
   public void HideUI()
   {
      SetUiState(false);
   }

   public void updateTime(float time)
   {
      timeTxt.text = time.ToString();
   }
   
   


}
