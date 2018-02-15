using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{

    private Vector3 startPos;
    public int MovementRange = 50;

    private void OnEnable()
    {
        startPos = transform.position;
    }

    public void OnDrag(Vector3 touch)
    {
        Vector3 newPos = Vector3.zero;
        newPos = touch - startPos;
        transform.position = Vector3.ClampMagnitude(newPos, MovementRange) + startPos;
    }

    public void EndDrag()
    {
        transform.position = startPos;
    }

    public void SetPos(Vector3 newPos)
    {
        startPos = newPos;
        this.transform.parent.gameObject.transform.position = startPos;
    }

    public Vector3 GetMovement()
    {
        Vector3 current = Camera.main.ScreenToWorldPoint(transform.position);
        Vector3 start = Camera.main.ScreenToWorldPoint(startPos);
        Vector3 screenPos = current - start;
        return screenPos;
    }

    public float GetMagnitude()
    {
        float magnitude = (transform.position - startPos).magnitude;
        return magnitude;
    }

}
