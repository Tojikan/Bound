using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class that handles movement for the player object


public class PlayerMovement : MonoBehaviour
{
    public GameObject colliderChild;                                                //Drag reference to the child object that contains the collider for wall detection
    public float moveSpeed = 1.0f;                                                  //player move speed
    public float distanceTo = Mathf.Epsilon;                                        //distance to target object. Player stops as soon as we enter this distance
    private float speedFraction = 1.0f;                                             //Reduce move speed by a fraction based on Joystick magnitude        
    private Animator anim;                                                          //Animator variable to control player animations
    private SpriteRenderer spriterender;                                            //Spriterender to flip character on X axis when moving a negative X direction
    private Vector2 newPosition;                                                    //Storage variable for the movement vector towards our target destination
    private Vector2 targetPosition;                                                 //Storage variable for the target position we're going for
    private TopDownCircleCollider2D col;                                            //variable for our Collision detection class
    private bool isMoving;                                                          //Bool to control if we're moving or not and to stop movement.
    private bool hasStarted;                                                        //Check if we started. Is set to true after the first call of SetMovement()
   
    //Access our speedFraction if we're not in Joystick movement to set it to 1
    public float SpeedFraction
    {
        get { return speedFraction; }
        set { speedFraction = value; }
    }


    //Initializes our movement vector and make sure we're not moving
    void Awake ()
    {
        anim = GetComponent<Animator>();
        spriterender = GetComponent<SpriteRenderer>();
        col = colliderChild.GetComponent<TopDownCircleCollider2D>();

	}

    //Get references to our components
    void Start()
    {
        isMoving = false;
        targetPosition = Vector2.zero;
        newPosition = Vector2.zero;
        hasStarted = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Get our next move based on current position versus our target position. Use normalized for constant speed. Recalculate every frame to stop jumps
        newPosition = (targetPosition - (Vector2)transform.position).normalized * (speedFraction * moveSpeed) * Time.deltaTime;

        //Calculate the remaining distance between current position and goal
        float remainingDistance = Vector2.Distance(transform.position, targetPosition);

        //If we're within our distanceTo, stop moving
        if (remainingDistance <= distanceTo)
        {

            isMoving = false;
        }

        //Check if we're moving
        if (isMoving)
        {

            //Temp variables to store our next movements
            Vector3 xMove = new Vector2(newPosition.x, 0);
            Vector3 yMove = new Vector2(0, newPosition.y);

            //Move on X axis, and then check collision           
            transform.position += xMove;
            col.CheckColliders();


            //Then move on Y axis and then check collision
            transform.position += yMove;
            col.CheckColliders();

        }

        //Set animation. If we haven't started yet, then sets the character to face downwards
        if (hasStarted)
            SetAnimation(newPosition.normalized.x, newPosition.normalized.y);
        else
            SetAnimation(0, -1);
    }

    

    //Sets the target position. Called in the player controller class
    public void SetMovement(Vector2 end)
    {
        //Tells us that we've started accepting movements and sets the bool if its false
        if (!hasStarted)
        {
            hasStarted = true;
        }
        isMoving = false;
        targetPosition = end;           
        isMoving = true;                
    }

    //Sets the speed fraction based on Joystick magnitude. Called for Joystick movement
    public void SetSpeed(float magnitude, float range)
    {
        float speedRate = magnitude / range;
        //So we can't go faster than move speed
        Mathf.Clamp(speedRate, 0, 1);
        speedFraction = speedRate;
    }


    //Sets our player animation. Called every frame after we make our movements
    //Relies on getting a normalized vector. Change animation every change of 0.25 of the y direction of our movement
    private void SetAnimation(float x, float y)
    {
        anim.SetFloat("normalX", x);
        anim.SetFloat("normalY", y);
        if (!isMoving)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
        }


        /**Old code for animating the zergling**/
        ////Flips our animation on the X axis. Don't require as many sprites
        //if (newPosition.x < 0){spriterender.flipX = true;}
        //else {spriterender.flipX = false;}
        ////Sets our animation rotation. The animator controller will change our player rotation as we move.
        //anim.SetFloat("Rotation", newPosition.normalized.y);
        ////sets our walking animation. If we stop at any time, stop walking
        //if (isMoving == false){anim.SetBool("Moving", false);}
        //else{anim.SetBool("Moving", true);}
    }


    //simple function to stop any movement. Can call from outside this class if they get a reference to PlayerMovement
    public void StopMove()
    {
        isMoving = false;
    }


}
