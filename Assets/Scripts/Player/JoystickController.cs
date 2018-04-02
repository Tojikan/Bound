using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that handles our Joystick and gets movement vectors back to the player
public class JoystickController : MonoBehaviour
{

    private Vector3 startPos;                                   //This is the center point of our joystick which is the reference to determine movement from
    public int MovementRange = 50;                              //This is the movement range of our joystick that determines how far it can move from the origin and also sets our magnitude


    //Sets our origin on enable. Just ensures we don't get a null center point
    private void OnEnable()
    {
        startPos = transform.position;
    }

    //This moves the joystick around. Takes in a parameter of a Vector3 and the joystick follows toward that touch
    public void OnDrag(Vector3 touch)
    {
        //New zero vector3
        Vector3 newPos = Vector3.zero;
        //Finds the vector from center to touch
        newPos = touch - startPos;
        //Moves the joystick and clamps it to a magnitude from our start position
        transform.position = Vector3.ClampMagnitude(newPos, MovementRange) + startPos;
    }

    //Moves joystick back to center
    public void EndDrag()
    {
        transform.position = startPos;
    }

    //Hide the joystick
    public void HideJoystick()
    {
        transform.parent.gameObject.SetActive(false);
    }

    //Sets the center and then moves the parent object to the center, moving this as well
    public void SetPos(Vector3 newPos)
    {
        startPos = newPos;
        this.transform.parent.gameObject.transform.position = startPos;
    }

    //Returns a movement vector based on joystick position
    public Vector3 GetMovement()
    {
        //Convert to screen position for movement
        Vector3 current = Camera.main.ScreenToWorldPoint(transform.position);
        Vector3 start = Camera.main.ScreenToWorldPoint(startPos);
        
        //Subtract joystick position from center position and then returns
        Vector3 screenPos = current - start;
        return screenPos;
    }

    //Gets the magnitude of joystickand center and returns it
    public float GetMagnitude()
    {
        float magnitude = (transform.position - startPos).magnitude;
        return magnitude;
    }

}
