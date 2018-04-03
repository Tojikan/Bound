using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundEngine
{ 
    //Base parent class for all non-obstacle event triggers in the game. 
    //Used to trigger certain events in the game, such as setting a new spawn point
    public class EventTrigger : MonoBehaviour
    {
        private Vector2 position;                       //Store position
        public bool startsEnabled;                      //Lets us know if enabled by default
        public bool showBox;                            //Allows us to draw a blue box in Editor and scene views
        public Sprite eventSprite;                      //Drag a reference to this sprite when setting up a new EventTrigger prefab. Needed for referencing an array index for saving/loading
        private BoxCollider2D collide;                  //Collider component



        #region Constructor
        //Constructor classes
        public virtual void ConstructTrigger(Vector3 pos, bool starts)
        {
            transform.position = pos;
            startsEnabled = starts;
        }
        #endregion


        //Destroys object
        public virtual void DestroyTrigger()
        {
            Destroy(gameObject);
        }

        //Draws a box if the showbox is enabled
        private void OnDrawGizmos()
        {
            if (showBox == true)
            {
                Gizmos.color = Color.blue;
                BoxCollider2D boundary = GetComponent<BoxCollider2D>();
                Gizmos.DrawCube(transform.position, boundary.size);
            }
        }

    }
}
