using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSubscriber : MonoBehaviour
{
    public int triggerTime = 50;
    private Animator animate;
    public AnimationClip explosionType;
    private SpriteRenderer sprite;


    private void Awake()
    {
        animate = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }


    // Use this for initialization
    void OnEnable ()
    {
        Timer.TimerEventHandler += Explode;
	}

    void Explode(int time)
    {

        if (time % triggerTime == 0)
        {
            Debug.Log("Boom");
            Debug.Log(time);
        }
    }
	
}
