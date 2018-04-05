using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

public class ProjectileTrap : Obstacle
{
    public AnimationClip explosionType;
    private SpriteRenderer sprite;
    private BoxCollider2D collide;
    private Animator animate;


    //Gets our references and sets our audio player
    protected override void Awake()
    {
        collide = GetComponent<BoxCollider2D>();
        animate = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        EnableObstacle();
    }
}
