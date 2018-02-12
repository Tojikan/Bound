using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

public class ExploderObstacle : MonoBehaviour
{
    public int countdown;                                                   //The time in milliseconds when the explosion is spawned
    public int loopLength;                                                  //The timer length in milliseconds. Timer loops over once we hit this number.
    public const float millisecond = 0.01f;                                 //Constant variable that sets our timer incremenets
    public AnimationClip explosionType;                                     //Explosion animation we are using
    public int timeleft;                                                    //The current time on the timer
    public bool showBox;                                                    //Draws a red box to illuminate the collider of the exploder
    public bool audioPlayer;                                                

    private BoxCollider2D collide;
    private Animator animate;
    private SpriteRenderer sprite;
    private AudioSource audioSource;



    //Constructor class that lets us initialize the variables for each new created instance of Exploder and then gets references to our components
    //Should be called when loaded into the game
    public void Initialize(int countdown, int loopLength)
    {
        this.countdown = countdown;
        this.loopLength = loopLength;
        collide = GetComponent<BoxCollider2D>();
        animate = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        collide = GetComponent<BoxCollider2D>();
        animate = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        BeginSequence();
    }

    //Start the sequence of this exploder. 
    public void BeginSequence()
    {
        StartCoroutine("Timer");
    }

    public void StopSequence()
    {
        StopCoroutine("Timer");
    }


    public void Explode()
    {
        sprite.enabled = true;
        animate.Play(explosionType.name, -1, 0);
        if (!audioPlayer)
            SoundManager.instance.PlaySingleTwo(audioSource.clip);
        else
            SoundManager.instance.PlaySingle(audioSource.clip);
    }

    public void EndExplosion()
    {
        sprite.enabled = false;
    }

    public void StartCollision()
    {
        collide.enabled = true;
    }

    public void EndCollision()
    {
        collide.enabled = false;
    }


    private void OnDrawGizmos()
    {
        if (showBox == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, collide.size);
        }
    }


    //Timer Coroutine to spawn explosions
    IEnumerator Timer()
    {
        timeleft = 0;       //initialize timer

        //Continuous loop
        while (true)
        {
            //Wait for one millisecond
            yield return new WaitForSeconds(millisecond);

            //Increment our timer by 1. This basically has the effect of counting in milliseconds
            timeleft += 1;

            //If we exceed or equal the length of the loop, reset timer to 0
            if (timeleft >= loopLength)
            {

                timeleft = 0;
            }

            //Instantiate the attached explosion type at the current position. 
            if (timeleft == countdown)
            {
                Explode();
            }
        }
    }


}
