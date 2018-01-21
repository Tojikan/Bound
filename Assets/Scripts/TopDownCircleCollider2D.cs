using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://mbzsunityrecipes.wordpress.com/2017/08/12/first-blog-post/
//Class that detects collisions via an overlap circle. Good for topdown collisions
//Call CheckColliders in any movement script in order to have collisions. This will offset the object by the collision distance
//Recommended to check collisions twice - on the X and Y axis separately. 
//Remember to add a circle collider to the object

public class TopDownCircleCollider2D : MonoBehaviour
{
    public LayerMask targetLayer;                                                   //Select the layer we are checking colissions on
    public float overlapRadius = 10f;                                               //Sets the distance at which we are detecting other colliders from
    

    private Collider2D[] neighbors;                                                 //Array to store colliders that we detect
    private int maxNeighbors = 16;                                                  //initialize array value
    private Collider2D thisCollider;                                                //storage for component
    

	// Use this for initialization
	void Start ()
    {
        //Gets a reference to this collider. Set to circle collider.
        thisCollider = transform.GetComponent<CircleCollider2D>();
	}

    //Function to check collisions and put an offset on any colliding objects. Returns true if it collides and false if no collisions
    public bool CheckColliders()
    {
        //Create a Collider2D array
        neighbors = new Collider2D[maxNeighbors];

        //Store the number of colliders and the colliders into the Collider2D array
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, overlapRadius, neighbors, targetLayer);

        //Loop over all the colliders in the array
        for (int i = 0; i < count; i++)
        {
            //Distance variable to calculate distance between current object and the colliders we detected
            ColliderDistance2D colDist = Physics2D.Distance(thisCollider, neighbors[i]);

            //check if we're overlapped
            if (colDist.isOverlapped)
            {
                //if so, calculate the displacement and then change our position
                Vector3 displace = colDist.normal * colDist.distance;
                transform.position += displace;
                return true;
            }
           
        }

        //returns false if no collision detected
        return false;
    }
}
