using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    float interval;
    [SerializeField]
    float totalTime;
    [SerializeField]
    float time;

    [SerializeField]
    TextMeshProUGUI currentTimeText;
    [SerializeField]
    Button resetButton;
    [SerializeField]
    Button startButton;
    [SerializeField]
    Button stopButton;
    [SerializeField]
    Button pauseButton;
    [SerializeField]
    Button resumeButton;

    // public events
    public event Action OnTimerStart;
    public event Action OnTimerStop;
    public event Action OnTimerPause;
    public event Action OnTimerResume;
    public event Action OnTimerReset;
    public event Action<float> OnTimerUpdate;
    public event Action<float> OnTimerComplete;

    Coroutine timer;

    // public variables
    public bool isPaused;

    public void Start()
    {
        resetButton.onClick.AddListener(HandleResetButtonClick);
        startButton.onClick.AddListener(HandleStartButtonClick);
        stopButton.onClick.AddListener(HandleStopButtonClick);
        pauseButton.onClick.AddListener(HandlePauseButtonClick);
        resumeButton.onClick.AddListener(HandleResumeButtonClick);

        Reset();
    }

    private void Reset()
    {
        isPaused = false;
        time = totalTime;
        UpdateTime();
    }

    IEnumerator RunTimer()
    {
        if (time < totalTime)
            Debug.Log("Resuming");

        while (!isPaused)
        {
            UpdateTime();

            time--;
            if (time < 0)
            {
                // timer has ended
                StopTimer();
            }

            yield return new WaitForSecondsRealtime(interval);
        }

        yield return null;
        Debug.Log("Paused");
    }

    void UpdateTime()
    {
        if (currentTimeText != null)
            currentTimeText.text = ":" + time.ToString("00");

        if (OnTimerUpdate != null)
            OnTimerUpdate(time);
    }

    void StartTimer()
    {
        timer = StartCoroutine(RunTimer());
    }

    void StopTimer()
    {
        if (OnTimerComplete != null)
            OnTimerComplete(time);

        if (timer != null)
            StopCoroutine(timer);

        timer = null;
        Reset();
    }

    void PauseTimer()
    {
        if (isPaused == true)
            return;

        isPaused = true;
        if (timer != null)
            StopCoroutine(timer);

        timer = null;
    }

    void ResumeTimer()
    {
        if (isPaused == false)
            return; 

        isPaused = false;
        StartTimer();
    }

    void HandleResetButtonClick()
    {
        Reset();
    }

    void HandleStartButtonClick()
    {
        StartTimer();
    }

    void HandleStopButtonClick()
    {
        StopTimer();
    }

    void HandlePauseButtonClick()
    {
        PauseTimer();
    }

    void HandleResumeButtonClick()
    {
        ResumeTimer();
    }
}
