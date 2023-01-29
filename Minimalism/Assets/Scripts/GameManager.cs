using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public bool paused = true;
    public CinemachineVirtualCamera startCam;
    public Animator uiAnim;

    public int scoreNum = 0;
    public int waveNum = 1;

    public TextMeshProUGUI[] score;
    public TextMeshProUGUI[] wave;
    // Start is called before the first frame update
    void Start()
    {
        gm = this;
        initialSettings();
    }

    public void initialSettings()
    {
        paused = true;
        startCam.Priority = 11;
        scoreNum = 0;
        waveNum = 1;
        SetScore();
    }

    public void addScore(int amount)
    {
        scoreNum += amount;
        SetScore();
    }

    void SetScore()
    {
        foreach(var v in score)
        {
            v.text = ""+scoreNum;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(paused && Input.GetKeyDown(KeyCode.Space))
        {
            paused = false;
            startCam.Priority = 9;
            uiAnim.SetTrigger("Start");
        }
    }
}
