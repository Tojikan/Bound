using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;
using System;

//Projectile obstacle type. Fires a projectile from this position. The actual projectile is a child object attached to this. This just controls the shooting aspect
public class ProjectileTrap : Obstacle
{
    private Projectile c_projectile;                                        //Reference to the projectile child object
    public enum ProjectileDirection                                         //This Enum controls which direction the projectile flies towards
    {
        Down,
        Left,
        Up,
        Right,
    }
    public ProjectileDirection direction = ProjectileDirection.Down;       //Default is down


    private void Start()
    {
        Transform trapTransform = GetComponent<Transform>();

        //Get the reference to the child's projectile component
        foreach (Transform child in trapTransform)
        {
            try
            {
               c_projectile = child.GetComponent<Projectile>();
            }
            catch (Exception e)
            {
                //If we can't get the projectile, destroy this instead. 
                Debug.LogError(e);
                Debug.LogError("Unable to get projectile component");
                DestroyObstacle();
                return;
            }
            //Set the original startposition and rotation of the projectile accordingly
            c_projectile.StartPosition = transform.position;
            c_projectile.Rotation = ((int)direction);
        }
        
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



    #region Trap Actions

    //Fires a projectile and plays a sound
    private void FireProjectile()
    {
        c_projectile.IsFiring = true;
        PlayAudio();
    }

    //Plays the fire projectile action when the triggertime is hit. 
    public override void TriggerObstacle(int timerTime)
    {
        base.TriggerObstacle(timerTime);

        int counter = timerTime % loopLength;

        if (counter == triggerTime)
            FireProjectile();
    }

    #endregion

}
