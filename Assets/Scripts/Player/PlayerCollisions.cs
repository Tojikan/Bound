using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundEngine
{
    //Scripts to handle method calling on collision detection. Each collision body part should have this.
    //Make sure all obstacle tags are formatted as "LethalBodyPart"
    public class PlayerCollisions : MonoBehaviour
    {
        private string targetBodyCollider;                                  //Set in Start. The string name of the tag to check for
        public Player playerParent;                                         //Drag reference to parent class here in order to call methods from it

        public enum BodyColliders                                           //This enum selects which body part we are colliding on in the editor window
        {
            Feet,
            Body,
            Head,
            FullBody,
            UpperBody,
            LowerBody,
            None,
        }
        public BodyColliders selectCollider;

        private void Start()
        {
            //Format of a tag should be LethalBodyPart
            targetBodyCollider = "Lethal" + selectCollider.ToString();  
        }

        //Collision will call back methods in the player parent object
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Finish")
            {
                playerParent.EndpointCollision();
            }

            //Match with the selected body collider
            if (collision.tag == targetBodyCollider)
            {
                playerParent.OnObstacleHit();
            }
        }
    }
}
