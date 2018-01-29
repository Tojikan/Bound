using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Parent class that handles all of the touch inputs. On receiving a touch, will call a virtual function that is defined in the children class
//OnTouch<>Anywhere functions are for taps that occur at any location - useful for in-game movement.
//OnTouch<> functions are used for specific buttons
//Game currently only handles single touch at a time. No multitouch feature needed. Yet. 

//Thanks to Devin Curry at Devination

public class TouchInput : MonoBehaviour
{
    // Update is called once per frame
    protected virtual void Update ()
    {
        //Checks for a touch input
        if (Input.touches.Length <= 0)
        {
            //calles NoTouches if no input
            NoTouches();
        }
        else
        {
            //Stores the first touch only. 
            Touch currTouch = Input.touches[0];

            //TO DO
            //IMPLEMENT A SYSTEM FOR RECEIVING TOUCHES ON A BUTTON
            //WILL WORK ON LATER ONCE I HAVE ACTUAL BUTTONS


            //Switch case for determining the current phase and which function to call
            switch (currTouch.phase)
            {
                //Call the Began Anywhere. For tap control movement, gives a single tap
                case TouchPhase.Began:
                    OnTouchBeganAnywhere();
                    break;

                //Call the Ended Anywhere
                case TouchPhase.Ended:
                    OnTouchEndedAnywhere();
                    break;

                //Call the Moved Anywhere. Used for drag control movement
                case TouchPhase.Moved:
                    OnTouchMovedAnywhere();
                    break;

                //Calls stationary movement
                case TouchPhase.Stationary:
                    OnTouchStayedAnywhere();
                    break;
            }
        }
	}


    //TO DO
   //GET THESE FUNCTIONS HOOKED UP TO A BUTTON
    


    protected virtual void NoTouches() { }
    protected virtual void OnTouchBegan() { print(name + " is not using OnTouchBegan"); }
    protected virtual void OnTouchEnded() { }
    protected virtual void OnTouchMoved() { }
    protected virtual void OnTouchStayed() { }


    //Virtual functions to be overridden in child classes
    protected virtual void OnTouchBeganAnywhere() { }
    protected virtual void OnTouchEndedAnywhere() { }
    protected virtual void OnTouchMovedAnywhere() { }
    protected virtual void OnTouchStayedAnywhere() { }
}
