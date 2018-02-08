using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public float explodeTime;

    private ExplosionTimer timer;
    private SpriteRenderer sprite;
    private Animator animator;
    private bool hasExploded;

    private void Start()
    {
        timer = GetComponentInParent<ExplosionTimer>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (timer.seconds == 0)
        {
            hasExploded = false;
        }

        if (timer.seconds >= explodeTime && hasExploded == false)
        {
            StartExplosion();
        }


    }
    
    void StartExplosion()
    {
        sprite.enabled = true;
        animator.SetTrigger("AnimationTrigger");

    }


    void StopExplosion()
    {
        sprite.enabled = false;
        hasExploded = true;
    }

}
