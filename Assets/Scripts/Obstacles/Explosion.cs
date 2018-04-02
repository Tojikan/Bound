using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

public class Explosion : Obstacle
{
    public AnimationClip explosionType;
    private SpriteRenderer sprite;
    private BoxCollider2D collide;
    private AudioSource audioSource;                                        //Reference to audiosource for explosion sFx
    private Animator animate;                                               //Reference our animator component

    //Enum to select which Audio player to play from
    public enum Audioplayers
    {
        sFxPlayerOne,
        sFxPlayerTwo,
        sFxPlayerThree,
        sFxPlayerFour,
        sFxPlayerFive
    }
    public Audioplayers SelectSFXPlayer = Audioplayers.sFxPlayerOne;

    //Gets our references and sets our audio player
    private void Awake()
    {
        collide = GetComponent<BoxCollider2D>();
        animate = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        EnableObstacle();
        Timer.TimerEventHandler += TriggerObstacle;
    }

    #region Constructor classes
    //Constructor classes that overrides obstacle parent class to initialize obstacle data. Should be called when an obstacle is loaded into the game
    //Defaulted audio player constructor
    public override void ConstructObstacle(int explosionTime, int loop)
    {
        triggerTime = explosionTime;
        loopLength = loop;
        SelectSFXPlayer = (Audioplayers)1;
    }

    //Overload constructor with parameter for the audio player
    public override void ConstructObstacle(int explosionTime, int loop, int audioPlayerNum)
    {
        triggerTime = explosionTime;
        loopLength = loop;
        SelectSFXPlayer = (Audioplayers)audioPlayerNum;
    }
    #endregion


    //Switches audio players based on which Audio player is selected. Defaults to AudioPlayer one. 
    public void PlayAudio()
    {
        switch ((int)SelectSFXPlayer)
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

    //Remove from event when disabled
    private void OnDisable()
    {
        Timer.TimerEventHandler -= TriggerObstacle;
    }

    //Redundancy to remove from event and then destroy obstacle
    public override void DestroyObstacle()
    {
        Timer.TimerEventHandler -= TriggerObstacle;
        base.DestroyObstacle();
    }

    #region Explosion Actions
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
    #endregion

    //Trigger function. Takes in a time parameter. 
    public override void TriggerObstacle(int timerTime)
    {
        if (CheckZero() == false)
            DestroyObstacle();

        int counter = timerTime % loopLength;

        if (counter == triggerTime)
            Explode();     
    }
}
