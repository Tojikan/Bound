using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int triggerTime;
    public bool isEnabled;

    public virtual void TriggerObstacle(int timerTime) { }

    public virtual void DisableObstacle()
    {
        isEnabled = false;
    }
    public virtual void EnableObstacle()
    {
        isEnabled = true;
    }

    public virtual void DestroyObstacle()
    {
        Destroy(gameObject);
    }

   
}
