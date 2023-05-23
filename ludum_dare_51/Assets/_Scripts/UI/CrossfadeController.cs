using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossfadeController : MonoBehaviour
{
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private Animator _anim;

    private void Awake()
    {
        fadePanel.SetActive(true);
        _anim = GetComponent<Animator>();
        fadePanel = transform.Find("Panel").gameObject;
    }

    public void StartCrossfade()
    {
        _anim.SetTrigger("start");
    }
    
}
