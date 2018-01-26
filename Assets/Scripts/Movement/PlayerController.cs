using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : TouchInput
{
    public bool movementEnabled;                                           //bool to check if we're supposed to move and not, such as during pauses and level starts/ends
    public bool dragPlay;                                                  //Bool to set if Drag Play controls are enabled

    
    private PlayerMovement movePlayer;                                     //reference to our component that moves the player

    // Use this for initialization
    void Start ()
    {
        //initialize bools and get reference to component
        movePlayer = GetComponent<PlayerMovement>();
        dragPlay = false;
        movementEnabled = true;
    }	

    protected override void OnTouchBeganAnywhere()
    {
        if (movementEnabled)
        {
            //checks if we're in tap play
            if (dragPlay == false)
            {
                //gets a single touch from the touch array - only single touch supported
                Touch myTouch = Input.touches[0];

                //converts the position of the touch to a world position
                Vector2 newPosition = Camera.main.ScreenToWorldPoint(myTouch.position);

                //Sets us to move by calling movePlayer
                movePlayer.SetMovement(newPosition);
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
                //stops any movement if we lift our finger during drag play
                movePlayer.StopMove();
            }
        }
    }



}
