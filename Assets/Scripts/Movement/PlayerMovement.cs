using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class that handles movement for the player object


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;                                                  //player move speed
    public float distanceTo = Mathf.Epsilon;                                        //distance to target object. Player stops as soon as we enter this distance
    private float speedFraction = 1.0f;                                             //Reduce move speed by a fraction based on Joystick magnitude        
    private Animator anim;                                                          //Animator variable to control player animations
    private SpriteRenderer spriterender;                                            //Spriterender to flip character on X axis when moving a negative X direction
    private Vector2 newPosition;                                                    //Storage variable for the movement vector towards our target destination
    private Vector2 targetPosition;                                                 //Storage variable for the target position we're going for
    private TopDownCircleCollider2D col;                                            //variable for our Collision detection class
    private PlayerController playerControl;                                         //Get Player controller component to see which movement style we're using
    private bool isMoving;                                                          //Bool to control if we're moving or not and to stop movement.
   
    //Access our speedFraction if we're not in Joystick movement
    public float SpeedFraction
    {
        get { return speedFraction; }
        set { speedFraction = value; }
    }


    //Initializes our movement vector and make sure we're not moving
    void Awake ()
    {
        targetPosition = transform.position;
		newPosition = Vector2.zero;
        isMoving = false;
	}

    //Get references to our components
    void Start()
    {
        anim = GetComponent<Animator>();
        spriterender = GetComponent<SpriteRenderer>();
        col = transform.GetComponent<TopDownCircleCollider2D>();
        playerControl = GetComponent<PlayerController>();
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

        //Set our animation accordingly
        AnimationHandler();
    }

    

    //Sets the target position. Called in the player controller class
    public void SetMovement(Vector2 end)
    {
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
    public void AnimationHandler()
    {

        //Flips our animation on the X axis. Don't require as many sprites
        if (newPosition.x < 0)
        {
            spriterender.flipX = true;
        }
        else
        {
            spriterender.flipX = false;
        }

        //Sets our animation rotation. The animator controller will change our player rotation as we move.
        anim.SetFloat("Rotation", newPosition.normalized.y);


        //sets our walking animation. If we stop at any time, stop walking
        if (isMoving == false)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
        }
    }


    //simple function to stop any movement. Can call from outside this class if they get a reference to PlayerMovement
    //May consider using a sendmessage depending if this'll work when we incorporate obstacles
    public void StopMove()
    {
        isMoving = false;
    }


}
