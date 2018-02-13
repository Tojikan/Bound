using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;
using UnityEditor;

public class ExploderObstacle : MonoBehaviour
{
    public int countdown;                                                   //The time in milliseconds when the explosion is spawned
    public int loopLength;                                                  //The timer length in milliseconds. Timer loops over once we hit this number.
    public const float millisecond = 0.01f;                                 //Constant variable that sets our timer incremenets
    public AnimationClip explosionType;                                     //Explosion animation we are using
    public int timeleft;                                                    //The current time on the timer
    public bool showBox;                                                    //Draws a red box to illuminate the collider of the exploder
    public int audioPlayer;                                                 //Sets which audio player we're going to use
    public bool isTesting;                                                  //Simple Bool for testing our maps when we're in the map editor scene. Remove later. 
    private BoxCollider2D collide;                                          //Reference our collider component
    private Animator animate;                                               //Reference our animator component
    private SpriteRenderer sprite;                                          //Reference our sprite renderer
    private AudioSource audioSource;                                        //Refernce our audio source component


    //Constructor class that lets us initialize the variables for each new created instance of Exploder and then gets references to our components
    //Should be called when an obstacle is loaded into the game
    public void Initialize(int countdown, int loopLength)
    {
        this.countdown = countdown;
        this.loopLength = loopLength;
    }

    //Overload constructor with parameter for the audio player
    public void Initialize(int countdown, int loopLength, int audioPlayerNum)
    {
        this.countdown = countdown;
        this.loopLength = loopLength;
        audioPlayer = audioPlayerNum;
    }
    

    //Gets our references and sets our audio player
    private void Awake()
    {
        collide = GetComponent<BoxCollider2D>();
        animate = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    //Switches audio players based on which Audio player is selected. Defaults to AudioPlayer one. 
    //TO DO: Maybe change from switchcase?
    public void PlayAudio()
    {
        switch(audioPlayer)
        {
            case 1:
                SoundManager.instance.AudioPlayerOne(audioSource.clip);
                break;
            case 2:
                SoundManager.instance.AudioPlayerTwo(audioSource.clip);
                break;
            case 3:
                SoundManager.instance.AudioPlayerThree(audioSource.clip);
                break;
            case 4:
                SoundManager.instance.AudioPlayerFour(audioSource.clip);
                break;
            case 5:
                SoundManager.instance.AudioPlayerFive(audioSource.clip);
                break;
            default:
                SoundManager.instance.AudioPlayerOne(audioSource.clip);
                break;
        }
    }

    //Start the timer of this exploder. 
    public void BeginSequence()
    {
        StartCoroutine("Timer");
    }

    //Stops the timer
    public void StopSequence()
    {
        StopCoroutine("Timer");
    }


    //Triggers our exploder to explode
    public void Explode()
    {
        //Re-enable the sprite renderer
        sprite.enabled = true;
        //Sets the animator to replay the animation from the beginning
        animate.Play(explosionType.name, -1, 0);
        //Plays our explosion sound based on which audio player is selected
        PlayAudio();
    }

    //Disables the sprite renderer to make the explosion animation stop being rendered. Called by an animation event
    public void EndExplosion()
    {
        sprite.enabled = false;
    }

    //Enables the collider for our explosion. Set in an animation event.
    public void StartCollision()
    {
        collide.enabled = true;
    }

    //Disables the collider, whenever you want to actual explosion to end. Called by animation event
    public void EndCollision()
    {
        collide.enabled = false;
    }

    //Used to draw big red boxes over the area of the collider, useful for editing/creating maps. Simply set to true/false in the editor window or in the explosion container editor
    private void OnDrawGizmos()
    {
        if (showBox == true)
        {
            Gizmos.color = Color.red;
            BoxCollider2D boundary = GetComponent<BoxCollider2D>();
            Gizmos.DrawCube(transform.position, boundary.size);
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
