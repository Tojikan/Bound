using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//Parent class that handles all of the touch inputs. On receiving a touch, will call a virtual function that is defined in the children class
//OnTouch<>Anywhere functions are for taps that occur at any location - useful for in-game movement.
//Will do a GraphicRaycaster to check if the touch is over a UI button
//TODO Fix Tap movement multiple input bug

//Thanks to Devin Curry at Devination

public class TouchInput : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;                                   //Raycaster component. Place on on the Canvas game object. Make sure you uncheck Raycast target on UI elements you don't want to check for
    PointerEventData m_PointerEventData;                            //PointerEventData container for your touch
    EventSystem m_EventSystem;                                      //Reference to Event system

    protected virtual void Awake()
    {
        //Find the GraphicRaycaster element on the canvas
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        //Get the EventSystem component in the scene
        m_EventSystem = GetComponent<EventSystem>();
    }


    // Update is called once per frame
    protected virtual void Update ()
    {
        //Checks for a touch input
        if (Input.touches.Length <= 0)
        {
            //calles NoTouches if no input
            //So far the only use is the bugfix for the joystick
            NoTouches();
        }
        
        else
        {
            //Gets the first touch from input
            Touch currentTouch = Input.GetTouch(0);
            //Detect to see if we are over a UI. 
            if (!DetectUI(currentTouch))
            {              
                //If no UI element below, call the touch functions
                TouchControls(currentTouch);
            }
        }
	}

    //Checks if there is a UI element that is a Raycast target beneath the touch
    bool DetectUI(Touch touch)
    {
        //Createa  new pointereventdata with our eventsystem and initialize its position
        m_PointerEventData = new PointerEventData(m_EventSystem)
        {   position = touch.position   };

        //List to store raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        //Cast the ray
        m_Raycaster.Raycast(m_PointerEventData, results);

        //Iterate over each result
        foreach (RaycastResult result in results)
        {
            //If any of the results has an index (i.e. there is a raycast target), then return true
            if (result.index == 0)
            {
                return true;
            }
        }
        return false;
    }

    //Has logic for determining which virtual function to call based on the touch phase
    void TouchControls(Touch currTouch)
    {
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

            case TouchPhase.Stationary:
                OnTouchStayedAnywhere();
                break;
        }
    }    

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
