using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Exploder class. This gameObject spawns explosions at their given location and handles the timing aspect of the game
public class Exploder : MonoBehaviour
{
    public int countdown;                                                   //The time in milliseconds when the explosion is spawned
    public int loopLength;                                                  //The timer length in milliseconds. Timer loops over once we hit this number.
    public const float millisecond = 0.01f;                                 //Constant variable that sets our timer incremenets
    public GameObject explosionType;                                        //Reference which explosion prefab we are creating
    public int timeleft;                                                    //The current time on the timer
    public bool showBox;
    

    //Constructor class that lets us initialize the variables for each new created instance of Exploder
    public void Initialize(int countdown,  int loopLength)
    {
        this.countdown = countdown;
        this.loopLength = loopLength; 
    }


    //Start the sequence of this exploder. 
    public void BeginSequence()
    {
        StartCoroutine("Timer", loopLength);
    }

    //Stops the timer
    public void StopSequence()
    {
        StopAllCoroutines(); 
    }

    private void OnDrawGizmos()
    {
        if (showBox == true)
        {
            Gizmos.color = Color.red;
            BoxCollider2D boundary = explosionType.GetComponent<BoxCollider2D>();
            Gizmos.DrawCube(transform.position, boundary.size);
        }
    }

    //Timer Coroutine to spawn explosions
    IEnumerator Timer(float loop)
    {
        timeleft = 0;       //initialize timer

        //Continuous loop
        while (true)
        {
            //Wait for one millisecond
            yield return new WaitForSeconds(millisecond);

            //Increment our timer by 1. This basically has the effect of counting in milliseconds
            timeleft += 1;

            //If we exceed or equal the length of the loop, reset timer to 0
            if (timeleft >= loopLength)
            {
                
                timeleft = 0;
            }

            //Instantiate the attached explosion type at the current position. 
            if (timeleft == countdown)
            {
                Instantiate(explosionType, transform.position, transform.rotation);
            }
        }
    }

}
