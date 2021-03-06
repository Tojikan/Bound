﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

//Basic obstacle that triggers an explosion at its location. Colliders and actions are set via animation events 
public class Explosion : Obstacle
{
    private SpriteRenderer sprite;                                          //sprite renderer component
    private BoxCollider2D collide;                                          //box collider component
    private Animator animate;                                               //Reference our animator component

    //Enum to select which Audio player to play from

    //Gets our references and sets our audio player
    protected override void Awake()
    {
        collide = GetComponent<BoxCollider2D>();
        animate = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        base.Awake();
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

    #region Explosion Actions
    //Triggers our exploder to explode
    private void Explode()
    {
        string explosionName = RemoveCharacters(animate.name);
        //Re-enable the sprite renderer
        sprite.enabled = true;
        //Sets the animator to replay the animation from the beginning
        animate.Play(explosionName, -1, 0);
        //Plays our explosion sound based on which audio player is selected
        PlayAudio();
    }

    private string RemoveCharacters(string name)
    {
        string stripped;
        int index = name.IndexOf(" ");
        if (index > 0)
            stripped = name.Substring(0, index);
        else
            stripped = name;
        return stripped;
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

    //EventTrigger function. Takes in a time parameter. 
    public override void TriggerObstacle(int timerTime)
    {
        base.TriggerObstacle(timerTime);

        int counter = timerTime % loopLength;

        if (counter == triggerTime)
            Explode();     
    }
}
