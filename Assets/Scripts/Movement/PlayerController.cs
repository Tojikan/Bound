using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoundEngine;

public class PlayerController : TouchInput
{
    public static bool movementEnabled;                                    //bool to check if we're supposed to move and not, such as during pauses and level starts/ends
    public float restartDelay = 1f;                                        //Delay between restarts;
    public JoystickController joystick;                                    //Drag joystick object here
    public GameObject joystickParent;                                      //Drag joystick parent object here
    private PlayerMovement movePlayer;                                     //reference to our component that moves the player
    private int lives;                                                     //Lives Count

    //Enum type to select the control options
    public enum ControlOptions
    {
        Drag,
        Joystick,
        Tap
    }

    public ControlOptions controlOptions = ControlOptions.Joystick;         //Selects which control option we are using
    private ControlOptions OldControlOptions;                               //Placeholder to see if we changed control schemes
    private ControlOptions SetControlOptions                                //Property Get;Set to set our oldcontroloptions but also calls the function that actually changes our controls
    {
        get { return OldControlOptions; }
        set
        {
            //This calls the function that will set our controls
            SetControls(controlOptions);
            OldControlOptions = value;
        }
    }

    #region Delegates
    //These 4 different delegates are called in response to different touch events.
    //Different movement methods are subscribed to each according to the movement scheme
    public delegate void OnTap();
    public static event OnTap OnTouchTap;

    public delegate void OnDrag();
    public static event OnDrag OnTouchDrag;

    public delegate void OnStay();
    public static event OnStay OnTouchStay;

    public delegate void OnEnd();
    public static event OnEnd OnTouchEnd;
    #endregion 


    // Use this for initialization
    void Start()
    {
        //initialize bools and get reference to component
        movePlayer = GetComponent<PlayerMovement>();
        //Set our initial controls
        SetControls(controlOptions);
        //Stores our old control options
        OldControlOptions = controlOptions;
    }

    protected override void Update()
    {
        base.Update();

        //Bit of code to let us do mouseclick for testing purposes in the game so we don't have to keep hooking up a phone. 
        //Don't forget to remove for builds
        if (movementEnabled)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                movePlayer.SetMovement(newPosition);
            }
        }

        //Checks to see if we have changed our controls since the last frame. If so, use the get;set to call the function to set controls
        if (OldControlOptions != controlOptions)
        {
            SetControlOptions = controlOptions;
        }
    }

    #region TouchEvent
    //A single tap is registered on the screen
    protected override void OnTouchBeganAnywhere()
    {
        if (movementEnabled)
        {
            if (OnTouchTap != null)
                OnTouchTap();
        }

    }

    //When the finger moves around on the screen
    protected override void OnTouchMovedAnywhere()
    {
        if (movementEnabled)
        {
            if (OnTouchDrag != null)
                OnTouchDrag();
        }
    }

    //If the touch stays on the same spot on the screen
    protected override void OnTouchStayedAnywhere()
    {
        if (movementEnabled)
        {
            if (OnTouchStay != null)
                OnTouchStay();
        }
    }

    //If the touch ends anywhere on the screen
    protected override void OnTouchEndedAnywhere()
    {
        if (movementEnabled)
        {
            if (OnTouchEnd != null)
                OnTouchEnd();
        }
    }
    #endregion

    //Method that sets the controls based on our enum choices
    public void SetControls(ControlOptions controls)
    {
        //Clears all events of subscribed methods
        OnTouchTap = null; OnTouchDrag = null; OnTouchStay = null; OnTouchEnd = null;

        /*Then subscribe methods to our delegates based on the type of movement we want to use*/
        
        if (controls == ControlOptions.Drag)
        {
            OnTouchDrag += MoveToTouch;
            OnTouchEnd += StopMovement;
            movePlayer.SpeedFraction = 1.0f;
        }

        if (controls == ControlOptions.Joystick)
        {
            OnTouchTap += CreateJoystick;
            OnTouchDrag += JoystickControls;
            OnTouchStay += JoystickControls;
            OnTouchEnd += JoystickOnEnd;
        }

        if (controls == ControlOptions.Tap)
        {
            OnTouchTap += MoveToTouch;
            movePlayer.SpeedFraction = 1.0f;
        }

    }

    #region Controls

    //Sets our player to move when we start moving the joystick
    private void JoystickControls()
    {
        if (joystickParent.activeInHierarchy == false)
        {
            CreateJoystick();
        }

        //Gets only a single touch
        Touch myTouch = Input.touches[0];

        //converts the position of the touch to a world position
        Vector3 touchPosition = myTouch.position;
        //Moves the Joystick based on touch position
        joystick.OnDrag(touchPosition);
        //Get a target position based on current
        Vector3 moveTo = joystick.GetMovement() + transform.position;
        //Sets our movespeed based on the magnitude of joystick from its origin point so our player moves slower if the joystick isn't pushed as far
        movePlayer.SetSpeed(joystick.GetMagnitude(), (float)joystick.MovementRange);
        //Move toward target position
        movePlayer.SetMovement(moveTo);
    }

    //Creates a joystick whenever the screen is touched
    private void CreateJoystick()
    {
        //Gets single touch
        Touch myTouch = Input.touches[0];
        //Save the position of the touch
        Vector3 touchPosition = myTouch.position;
        //Set Joystick active so it'll show up. Default should be inactive
        joystickParent.SetActive(true);
        //Moves the Joystick to our touch position. Both touchposition and joysticks work on screen space so we don't need to convert to worldspace
        joystick.transform.position = touchPosition;
        //Sets the Joystick center point 
        joystick.SetPos(touchPosition);
    }

    //Hides the joystick when the touch is lifted
    private void JoystickOnEnd()
    {
        //Moves joystick back to its center
        joystick.EndDrag();
        //Stops player movement
        StopMovement();
        //Set joystick inactive
        joystick.HideJoystick();
    }

    //Simple tap movement that moves our player towards finger position. Used either in Drag or Tap play
    private void MoveToTouch()
    {
        //Gets only a single touch
        Touch myTouch = Input.touches[0];

        //converts the position of the touch to a world position
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(myTouch.position);

        //Sets us to move by calling movePlayer
        movePlayer.SetMovement(newPosition);
    }

    //To stop movement for our player.
    public void StopMovement()
    {
        movePlayer.StopMove();
        //Set joystick inactive
        joystick.HideJoystick();
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
    #endregion

}
