﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonphysicsMovement : MonoBehaviour {
    public float speed = 10f;                                                   //Movement speed variable
    public LayerMask blockingLayer;                                             //layer on which we detect collisions

    private BoxCollider2D boxCollider;                                          //BoxCollider storage variable
    private Vector3 targetPosition;                                             //Stores the end position that the player is moving to
    private bool isMoving;                                                      //Bool to control movement. When true, player begins moving. When reaching a point, becomes false
    private RaycastHit2D hit;                                                   //Store the linecast to detect collisions
    

    // Use this for initialization
    void Start ()
    {
        boxCollider = GetComponent<BoxCollider2D>();                            //Get a reference to collider component
        targetPosition = transform.position;                                    //get the current position of player and store it as the target position
        isMoving = false;                                                       //initialize to false so no movement
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Check if there are touches. If there is, call the SetTargetPosition function to get the position of the first touch
		if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            SetTargetPosition(myTouch);
        }

        //If isMoving is True, player uses begins to move
        if (isMoving)
        {
            MovePlayer();
        }

	}

    //Set the target position given a touch input
    void SetTargetPosition(Touch currTouch)
    {
        //Store the current position of player
        Vector2 CurrentPosition = transform.position;

        //Get the target position given by the touch input and convert to world point from a screen point
        Vector2 TargetPosition = Camera.main.ScreenToWorldPoint(currTouch.position);

        //disable the player collider to make sure the raycast doesn't hit our own collider
        boxCollider.enabled = false;

        //line cast to detect any collisions between the current position to target one on the blocking layer
        hit = Physics2D.Linecast(CurrentPosition, TargetPosition, blockingLayer);

        //re-enable the player collider
        boxCollider.enabled = true;

        //if we don't hit anything, set the target position to the one given by the touch
        if (hit.transform == null)
        {
            targetPosition = TargetPosition; 
        }


        //call the HitObject function to return a new position 
        else
        {
            HitObject();
        }

        //Set player to move
        isMoving = true;
 
    }

    //Returns a new position for player to move along the previous vector but before hitting the object
    void HitObject()
    {
    
        //TODO - IMPLEMENT A BETTER VERSION OF THIS METHOD
    
        Transform targetObject = hit.transform;
        Vector3 newTarget = hit.point;
        //BoxCollider2D objectCollider = targetObject.GetComponent<BoxCollider2D>();


        if (hit.point.x > targetObject.position.x)
            newTarget.x += this.GetComponent<BoxCollider2D>().bounds.extents.x;

        if (hit.point.x < targetObject.position.x)
            newTarget.x -= this.GetComponent<BoxCollider2D>().bounds.extents.x;

        if (hit.point.y > targetObject.position.y)
            newTarget.y += this.GetComponent<BoxCollider2D>().bounds.extents.y;

        if (hit.point.y < targetObject.position.y)
            newTarget.y -= this.GetComponent<BoxCollider2D>().bounds.extents.y;

        targetPosition = newTarget;
    }


    //Moves the player to the target position
    void MovePlayer()
    {
        //gets the current position of the player
        Vector2 currentPos = transform.position;

        //moves the player toward the target position
        transform.position = Vector2.MoveTowards(currentPos, targetPosition, speed * Time.deltaTime);

        //stops moving if we reach the target position
        if(transform.position == targetPosition)
        {
            isMoving = false;
        }

        //debug line for use in scene view to see where we're moving to
        Debug.DrawLine(transform.position, targetPosition, Color.red);

    }
}
