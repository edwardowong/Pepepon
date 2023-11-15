using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;
    public TextMeshProUGUI TMPtimer;

    private TimeSpan gameTime;
    private float elapsedTime;
    private bool timerOn;
    

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;
    }

    public void BeginTimer()
    {
        timerOn = true;
        elapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }

    public void ResumeTimer()
    {
        timerOn = true;
        StartCoroutine(UpdateTimer());
    }

    public void PauseTimer()
    {
        timerOn = false;
        StopCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timerOn)
        {
            elapsedTime += Time.deltaTime;
            gameTime = TimeSpan.FromSeconds(elapsedTime);
            TMPtimer.text = "Final Time: " + gameTime.ToString("mm':'ss'.'ff");
            yield return null;
        }
    }
}
