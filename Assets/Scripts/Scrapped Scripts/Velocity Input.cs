//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class VelocityInput : TouchInput
//{
//    public float moveSpeed = 7f;                                            //Variable to control player move speed
//    public float stopDistance = 0.05f;                                      //Set a stopping distance between player and end point. Player begins slowing down as soon as we reach a destination
//    public bool dragPlay;                                                   //Bool to set if Drag Play controls are enabled

//    private Rigidbody2D rb;                                                 //Holds our rigidbody reference
//    private Vector2 targetPosition;                                         //Holds the position that our player will move to. Initialized at the current position in Start()                 
//    private bool isMoving;                                                  //Check that sees if we're moving or not. Setting to false will stop movement
//    private bool movementEnabled;


//    // Use this for initialization
//    void Start()
//    {
//        //initialize our starting position, makes sure we're not moving, gets a reference to the rigidbody component and sets our control scheme
//        rb = GetComponent<Rigidbody2D>();
//        targetPosition = transform.position;
//        isMoving = false;
//        dragPlay = false;
//        movementEnabled = true;
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {

//    }

//    //Enabled only for tap gameplay. When we receive a tap, begins setting a position to move to
//    protected override void OnTouchBeganAnywhere()
//    {
//        if (movementEnabled == false)
//            return;

//        //checks if we're in tap play
//        if (dragPlay == false)
//        {
//            //stops current movement
//            isMoving = false;

//            //gets a single touch from the touch array - only single touch supported
//            Touch myTouch = Input.touches[0];

//            //converts the position of the touch to a world position
//            targetPosition = Camera.main.ScreenToWorldPoint(myTouch.position);

//            //Sets us to move
//            isMoving = true;
//        }

//    }

//    //enabled only for drag gameplay. Sets a new position when we move our finger
//    protected override void OnTouchMovedAnywhere()
//    {
//        if (movementEnabled == false)
//            return;

//        //checks if we're in drag play
//        if (dragPlay == true)
//        {
//            //Gets only a single touch
//            Touch myTouch = Input.touches[0];

//            //converts the position of the touch to a world position
//            targetPosition = Camera.main.ScreenToWorldPoint(myTouch.position);

//            //Sets us to move
//            isMoving = true;
//        }
//    }

//    //enabled only for drag gameplay. Ends any movement if we lift up our finger
//    protected override void OnTouchEndedAnywhere()
//    {
//        if (movementEnabled == false)
//            return;

//        //checks if we're in drag play
//        if (dragPlay == true)
//        {
//            //stops movement
//            isMoving = false;
//        }
//    }

//}
