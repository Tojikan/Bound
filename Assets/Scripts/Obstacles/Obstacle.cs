using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Inherited base class for all obstacles
namespace BoundEngine
{
    public class Obstacle : MonoBehaviour
    {
        public int triggerTime;                     //Time when obstacle is triggered
        public int loopLength;                      //When the sequence loops back around
        public bool isEnabled;                      //bool check to enable or disable


        //virtual function for each obstacle. Triggers obstacle at the time. Takes in a parameter of current time
        public virtual void TriggerObstacle(int timerTime) { }                          

        //Disable
        public virtual void DisableObstacle()
        {
            isEnabled = false;
        }

        //Enable
        public virtual void EnableObstacle()
        {
            isEnabled = true;
        }

        //Checks if the length of the loop is zero, which is invalid
        public virtual bool CheckZero()
        {
            if (loopLength == 0)
            {
                Debug.Log("Invalid loop length.");
                return false;
            }
            return true;
        }

        //Destroys the obstacle
        public virtual void DestroyObstacle()
        {
            Destroy(gameObject);
        }
    }
}
