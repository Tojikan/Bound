using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Inherited base class for all obstacles
namespace BoundEngine
{
    public class Obstacle : MonoBehaviour
    {
        public bool showBox;
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

        //Constructor classes - has multiple overloads to reflect different obstacle types which might accept different parameters
 #region Constructor classes
        public virtual void ConstructObstacle(int time, int loop){}

        public virtual void ConstructObstacle(int time, int loop, int audio){}

#endregion

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

        //Used to draw big red boxes over the area of the collider, useful for editing/creating maps. Simply set to true/false in the editor window or in the explosion container editor
        private void OnDrawGizmos()
        {
            if (showBox == true)
            {
                Gizmos.color = Color.red;
                BoxCollider2D boundary = GetComponent<BoxCollider2D>();
                Gizmos.DrawCube(transform.position, boundary.size);
            }
        }
    }
}
