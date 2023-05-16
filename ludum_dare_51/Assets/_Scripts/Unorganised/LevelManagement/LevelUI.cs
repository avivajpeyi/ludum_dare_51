using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelUI : MonoBehaviour
{
   
   [SerializeField] public Button startGameButton;
   [SerializeField] public GameObject levelStartUi;
   [SerializeField] public TMP_Text timeTxt;
   [SerializeField] public TMP_Text roomTxt;
   [SerializeField] public TMP_Text playButtonTxt;
   [SerializeField] public TMP_Text levelTxt;
   [SerializeField] public Animator hourglassAnimator;
   private GameEventManager _gameEventManager;

   private void Awake()
   {
      ShowStartGameUi(); // start button
      _gameEventManager = FindObjectOfType<GameEventManager>();
      // startGameButton = levelStartUi.GetComponentInChildren<Button>();
      // timeTxt = transform.Find("FramePanel/timePanel/timeText").GetComponent<TMP_Text>();
      // roomTxt = transform.Find("FramePanel/roomPanel/roomText").GetComponent<TMP_Text>();
      // levelTxt = transform.Find("FramePanel/levelTxt").GetComponent<TMP_Text>();
      // playButtonTxt = startGameButton.GetComponentInChildren<TMP_Text>();
      // levelStartUi = transform.Find("FramePanel/StartLevelUi").gameObject;
      
      startGameButton.onClick.AddListener(_gameEventManager.RunStartGame);
      hourglassAnimator.SetBool("startTimer", false);
   }

   private void OnEnable()
   {
      _gameEventManager.OnStartGame += HideStartGameUi;
      _gameEventManager.OnEndGame += ShowEndGameUi;
      _gameEventManager.OnEndLevel += ShowEndLevelTxt;
      _gameEventManager.OnStartLevel += ShowStartLevelTxt;
   }
    
   private void OnDisable()
   {
      _gameEventManager.OnStartGame -= HideStartGameUi;
      _gameEventManager.OnEndGame -= ShowEndGameUi;
      _gameEventManager.OnEndLevel -= ShowEndLevelTxt;
      _gameEventManager.OnStartLevel -= ShowStartLevelTxt;
   }

   
   public void ShowStartGameUi()
   {
       levelStartUi.SetActive(true);
       playButtonTxt.text = "Start\n Dreaming?";
   }

   public void ShowEndGameUi()
   {
      levelStartUi.SetActive(true);
      playButtonTxt.text = "Steve Woke Up!\n Snooze?";
   }

   public void HideStartGameUi()
   {
      levelStartUi.SetActive(false);
   }

   public void updateTime(float time)
   {
      timeTxt.text = time.ToString();
   }
   
   public void updateRoomNumber(int count)
   {
      roomTxt.text = "Room " + count.ToString("00");
   }
   
   
   void ShowStartLevelTxt()
   {
      showLevelTxt("Stay asleep");
      hourglassAnimator.SetBool("startTimer", true);
   }
   
   void ShowEndLevelTxt()
   {
      showLevelTxt("Phew -- made it");
      hourglassAnimator.SetBool("startTimer", false);
   }

   public void showLevelTxt(string txt)
   {
      levelTxt.gameObject.SetActive(true);
      levelTxt.text = txt;
      levelTxt.color = new Color(levelTxt.color.r, levelTxt.color.g, levelTxt.color.b, 1);
      StartCoroutine(fadeOutTxt(levelTxt));
   }
   
   private IEnumerator fadeOutTxt(TMP_Text txt, float duration=2f)
   { 
      float currentTime = 0f;
      while(currentTime < duration)
      {
         float alpha = Mathf.Lerp(1f, 0f, currentTime/duration);
         txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha);
         currentTime += Time.deltaTime;
         yield return null;
      }
      txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
      txt.gameObject.SetActive(false);
   }




}
