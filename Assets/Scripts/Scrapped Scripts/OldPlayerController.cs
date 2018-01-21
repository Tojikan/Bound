using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to handle player movement 
//Inherits from Touch Input Class to handle touch input phases

public class PlayerController2 : TouchInput
{
    public float moveSpeed = 7f;                                            //Variable to control player move speed
    public float stopDistance = 0.05f;                                      //Set a stopping distance between player and end point. Player begins slowing down as soon as we reach a destination
    public bool dragPlay;                                                   //Bool to set if Drag Play controls are enabled
    public bool movementEnabled;                                           //bool to check if we're supposed to move and not, such as during pauses and level starts/ends
    public Animator anim;

    private Rigidbody2D rb;                                                 //Holds our rigidbody reference
    private Vector2 targetPosition;                                         //Holds the position that our player will move to. Initialized at the current position in Start()                 
    private bool isMoving;                                                  //Check that sees if we're moving or not. Setting to false will stop movement
    private SpriteRenderer spriterender;

    // Use this for initialization
    void Start()
    {

        //initialize our starting position, makes sure we're not moving, gets a reference to the rigidbody component and sets our control scheme
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriterender = GetComponent<SpriteRenderer>();
        targetPosition = transform.position;
        isMoving = false;
        dragPlay = false;
        movementEnabled = true;
        
    }

    // FixedUpdate is called during physics calculations. As we're using rigidbody move position, we use fixed update for more smooth movement
    void FixedUpdate()
    {
        if (movementEnabled)
        {
            //Makes sure we stop moving if we reach our destination
            if ((Vector2)transform.position == targetPosition)
            {
                isMoving = false;
                anim.SetTrigger("stoppedMoving");
            }

            //otherwise calls the move function
            if (isMoving == true)
            {
                Move();
            }
        }
    }

    //Enabled only for tap gameplay. When we receive a tap, begins setting a position to move to
    protected override void OnTouchBeganAnywhere()
    {
        if (movementEnabled)
        {
            //checks if we're in tap play
            if (dragPlay == false)
            {
                //stops current movement
                isMoving = false;
                anim.SetTrigger("stoppedMoving");





                //gets a single touch from the touch array - only single touch supported
                Touch myTouch = Input.touches[0];

                //converts the position of the touch to a world position
                targetPosition = Camera.main.ScreenToWorldPoint(myTouch.position);

                //Sets us to move
                isMoving = true;

                SetAnimation(targetPosition); 
                anim.SetTrigger("startedMoving");
            }
        }
    }

    //enabled only for drag gameplay. Sets a new position when we move our finger
    protected override void OnTouchMovedAnywhere()
    {
        if (movementEnabled)
        {
            //checks if we're in drag play
            if (dragPlay == true)
            {
                //Gets only a single touch
                Touch myTouch = Input.touches[0];

                //converts the position of the touch to a world position
                targetPosition = Camera.main.ScreenToWorldPoint(myTouch.position);

                //Sets us to move
                isMoving = true;
            }
        }
    }

    //enabled only for drag gameplay. Ends any movement if we lift up our finger
    protected override void OnTouchEndedAnywhere()
    {
        if (movementEnabled)
        {
            //checks if we're in drag play
            if (dragPlay == true)
            {
                //stops movement
                isMoving = false;
            }
        }
    }


    //gets us a position to move to every time we move. Called by the move function, movePosition only allows for incremental moves, so we must do it piecemeal
    Vector2 GetNextMove(Vector2 end)
    {   
        //holder variable
        Vector2 move;

        //Store the current position of player
        Vector2 CurrentPosition = transform.position;

        //Gets the distance from current position to target position
        float remainingDistance = Vector2.Distance(CurrentPosition, targetPosition);

        
        if (remainingDistance > stopDistance)
        {
            //If we're above stopping distance, move a normalized vector length instead. This prevents us from slowing down too much as we reach the end
            move = (targetPosition - CurrentPosition).normalized * moveSpeed * Time.deltaTime;
        }

        else
        {
            //Return to movement by deltaTime. Lets us actually reach the destination and not overshoot it by a normalized vector
            move = (targetPosition - CurrentPosition) * moveSpeed * Time.deltaTime;
            anim.SetTrigger("stoppedMoving");
        }

        //returns a position to move to
        return move;
    }


    //Move function is called in every physics frame whenever isMoving is true
    void Move()
    {
        //temporary storage to get our next move from GetNextMove
        Vector2 movementPosition = GetNextMove(targetPosition);

        //moves our position from current position to the new movement position
        rb.MovePosition(rb.position + movementPosition);


        //This line draws a redline in the sceneview
        //DONT FORGET TO REMOVE EVENTUALLY
        Debug.DrawLine(transform.position, targetPosition, Color.red); 
        
    }


    void SetAnimation(Vector2 dest)
    {
        Vector2 dir = dest.normalized;
        float rotate = dir.x;
        int angle = (int)(dir.y * 100);
         

        if (rotate < 0)
        {
            spriterender.flipX = true;
        }
        else
        {
            spriterender.flipX = false;
        }
        anim.SetInteger("direction", angle);

    }

}
