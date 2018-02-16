﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Timer instance;
    int time;
    const float millisecond = 0.01f;


    public delegate void TimerEvents(int eventTime);
    public static event TimerEvents TimerEventHandler;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    // Use this for initialization
    void Start ()
    {
        StartCoroutine("TimerRoutine");
	}

    //Timer Coroutine to spawn explosions
    IEnumerator TimerRoutine()
    {
        int time = 0;

        //Continuous loop
        while (true)
        {
            //Wait for one millisecond
            yield return new WaitForSeconds(millisecond);

            //Increment our timer by 1. This basically has the effect of counting in milliseconds
            time += 1;
            if (TimerEventHandler != null)
                TimerEventHandler(time);
        }
    }
}
