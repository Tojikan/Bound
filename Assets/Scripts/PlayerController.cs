using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : TouchInput
{
    public static bool movementEnabled;                                    //bool to check if we're supposed to move and not, such as during pauses and level starts/ends
    public bool dragPlay;                                                  //Bool to set if Drag Play controls are enabled
    public float restartDelay = 1f;                                        //Delay between restarts;
    public JoystickController joystick;                                    //Drag joystick object here
    public GameObject joystickParent;                                      //Drag joystick parent object here
    private PlayerMovement movePlayer;                                     //reference to our component that moves the player
    private int lives;                                                     //Lives Count
    
    public enum ControlOptions
    {
        Drag,
        Joystick,
        Tap
    }

    public ControlOptions controlOptions = ControlOptions.Joystick;


    // Use this for initialization
    void Start ()
    {
        //initialize bools and get reference to component
        movePlayer = GetComponent<PlayerMovement>();
        dragPlay = false;
    }

    protected override void Update()
    {
        base.Update();

        //Bit of code to let us do mouseclick for testing purposes in the game so we don't have to keep hooking up a phone. 
        //Don't forget to remove for builds
        if (movementEnabled)
        {
            //if (dragPlay == false)
            //{
            //    if(Input.GetMouseButtonDown(1))
            //    {
            //        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //        movePlayer.SetMovement(newPosition);
            //    }
            //}
        }
    }


    protected override void OnTouchBeganAnywhere()
    {
        if (movementEnabled)
        {
            ////checks if we're in tap play
            //if (dragPlay == false)
            //{
            //    //gets a single touch from the touch array - only single touch supported
            //    Touch myTouch = Input.touches[0];

            //    //converts the position of the touch to a world position
            //    Vector2 newPosition = Camera.main.ScreenToWorldPoint(myTouch.position);

            //    //Sets us to move by calling movePlayer
            //    movePlayer.SetMovement(newPosition);
            //}

            if (dragPlay == false)
            {
                Touch myTouch = Input.touches[0];
                Vector3 touchPosition = myTouch.position;
                joystickParent.SetActive(true);
                joystick.transform.position = touchPosition;
                joystick.SetPos(touchPosition);
                joystick.OnDrag(touchPosition);
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
                Vector2 newPosition = Camera.main.ScreenToWorldPoint(myTouch.position);

                //Sets us to move by calling movePlayer
                movePlayer.SetMovement(newPosition);
            }
            else
            {
                //Gets only a single touch
                Touch myTouch = Input.touches[0];

                //converts the position of the touch to a world position
                Vector3 touchPosition = myTouch.position;
                joystick.OnDrag(touchPosition);

                Vector3 moveTo = joystick.GetMovement() + transform.position;
                movePlayer.SetSpeed(joystick.GetMagnitude(), (float)joystick.MovementRange);
                movePlayer.SetMovement(moveTo);
            }
        }
    }

    protected override void OnTouchStayedAnywhere()
    {
        //Gets only a single touch
        Touch myTouch = Input.touches[0];

        //converts the position of the touch to a world position
        Vector3 touchPosition = myTouch.position;
        joystick.OnDrag(touchPosition);
        Vector3 moveTo = joystick.GetMovement() + transform.position;
        movePlayer.SetMovement(moveTo);
    }

    //enabled only for drag gameplay. Ends any movement if we lift up our finger
    protected override void OnTouchEndedAnywhere()
    {
        if (movementEnabled)
        {
            //checks if we're in drag play
            if (dragPlay == true)
            {
                //stops any movement if we lift our finger during drag play
                StopMovement();
            }
            else
            {
                joystick.EndDrag();
                StopMovement();
                joystickParent.SetActive(false);
            }
        }
    }

    //To stop movement for our player.
    public void StopMovement()
    {
        movePlayer.StopMove();
    }

    //These functions need to be called if you're enabling or disabling movement, as it is a static bool
    public void DisableMovement()
    {
        movementEnabled = false;
    }

    public void EnableMovement()
    {
        movementEnabled = true;
    }
}
