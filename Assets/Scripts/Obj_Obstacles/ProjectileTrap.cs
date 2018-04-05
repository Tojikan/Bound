using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;
using System;

public class ProjectileTrap : Obstacle
{
    private Projectile c_projectile;
    public enum ProjectileDirection
    {
        Down,
        Left,
        Up,
        Right,
    }
    public ProjectileDirection direction = ProjectileDirection.Down;


    private void Start()
    {
        Transform trapTransform = GetComponent<Transform>();

        foreach (Transform child in trapTransform)
        {
            try
            {
               c_projectile = child.GetComponent<Projectile>();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Unable to get projectile component");
                DestroyObstacle();
                return;
            }
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

    private void FireProjectile()
    {
        if (!c_projectile.IsFiring)
            c_projectile.IsFiring = true;
        c_projectile.ResetPosition();
        PlayAudio();
    }

    public override void TriggerObstacle(int timerTime)
    {
        base.TriggerObstacle(timerTime);

        int counter = timerTime % loopLength;

        if (counter == triggerTime)
            FireProjectile();
    }

    #endregion

}
