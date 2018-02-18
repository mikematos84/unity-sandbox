using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerListener : MonoBehaviour {

    Timer timer;

	// Use this for initialization
	void Start () {
        timer = GetComponent<Timer>();

        timer.OnTimerUpdate += HandleTimerUpdate;
        timer.OnTimerComplete += HandleTimerComplete; ;
	}

    private void HandleTimerComplete(float obj)
    {
        Debug.Log("Complete");
    }

    private void HandleTimerUpdate(float obj)
    {
        Debug.Log(string.Format("Time: {0}", obj.ToString("00")));
    }
}
