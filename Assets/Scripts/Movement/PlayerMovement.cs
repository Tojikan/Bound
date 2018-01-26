using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class that handles movement for the player object


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;                                                  //player move speed
    public float distanceTo = 0.4f;                                                 //distance to target object. Player stops as soon as we enter this distance
    public float slideDistance = 1.0f;

    private Animator anim;                                                          //Animator variable to control player animations
    private SpriteRenderer spriterender;                                            //Spriterender to flip character on X axis when moving a negative X direction
    private Vector2 newPosition;                                                    //Storage variable for the movement vector towards our target destination
    private Vector2 targetPosition;                                                 //Storage variable for the target position we're going for
    private TopDownCircleCollider2D col;                                            //variable for our Collision detection class
    private bool isMoving;                                                          //Bool to control if we're moving or not and to stop movement.
   

    // Use this for initialization
    void Awake ()
    {
        //Initialize our movement vector and make sure we're not moving
        targetPosition = transform.position;
		newPosition = Vector2.zero;
        isMoving = false;
	}

    // Use this for initialization
    void Start()
    {

        //Get references to our components
        anim = GetComponent<Animator>();
        spriterender = GetComponent<SpriteRenderer>();
        col = transform.GetComponent<TopDownCircleCollider2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        //Get our next move based on current position versus our target position. Use normalized for constant speed. Recalculate every frame to stop jumps
        newPosition = (targetPosition - (Vector2)transform.position).normalized * moveSpeed * Time.deltaTime;

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


    //Called by our controller function, which handles our touch inputs and converts to world position. This is called anytime there is a touch input
    //Takes in a parameter of the coordinates of our touch. Then sets us to move
    public void SetMovement(Vector2 end)
    {
        isMoving = false;
        targetPosition = end;            //Update current target position
        isMoving = true;                 //Start moving 
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
