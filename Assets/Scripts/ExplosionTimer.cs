using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTimer : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Animator animator;

    public float TimeIncrements;
    public float seconds;
    public float loop;
    public float TimeToExplode;

    private bool HasExploded;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        StartCoroutine("Timer", loop);
    }

    IEnumerator Timer(float loop)
    {
        seconds = 0;
        while(true)
        {
            yield return new WaitForSeconds(TimeIncrements);
            seconds += TimeIncrements;

            if (seconds >= TimeToExplode && HasExploded == false)
            {
                animator.SetTrigger("AnimationTrigger");
                HasExploded = true;
            }


            if (seconds >= loop)
            {
                seconds = 0;
                HasExploded = false;
            }
        }
    }
}
