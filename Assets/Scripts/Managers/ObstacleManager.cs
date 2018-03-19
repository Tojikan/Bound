using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;

namespace BoundEngine
{
 
    //Class to handle the creation and controls of all our obstacles
    public class ObstacleManager : MonoBehaviour
    {
        public bool ShowBoxes;                                                       //Draw explosion collision boxes for debugging
        private List<Explosion> explosionList;                                       //List of all exploders currently in the scene. Use this to control the obstacles

        //Method to create our exploders. Takes in parameters of a list of obstacles and the explosion set
        public void CreateExploders(List<ObstacleData> explosions, ObstacleSet set)
        {
            //Create a new list
            explosionList = new List<Explosion>();

            //Iterate over each explosiondata in the mapfile explosion list
            foreach (ObstacleData data in explosions)
            {
                //Creates an exploder prefab at the specified position in the file
                GameObject newBomb = Instantiate(set.ObstaclePrefabs[data.obstacleType], data.position, transform.rotation);

                //Get a reference to the exploder component of our newly created prefab
                Explosion exploder = newBomb.GetComponent<Explosion>();    

                //Initialize the prefab data
                exploder.InitializeExplosion(data.triggerTime, data.loopLength, data.audioPlayer);
                //Adds this expoder to our list for controls
                explosionList.Add(exploder);
            }
        }

        //Iterates over each exploder in the list and destroys their GameObject, clearing all Obstacles. 
        public void ClearObstacles()
        {
            foreach (Explosion child in explosionList)
            {
                child.DestroyObstacle();
            }
        }

        //Draws the Gizmos square over each exploder
        void ShowAllBoxes()
        {
            //Check if bool is enabled
            if (ShowBoxes)
            {
                foreach (Explosion bomb in explosionList)
                {
                    bomb.showBox = true;
                }
            }
            //Make sure the box doesn't show if it's not true
            else
            {
                foreach (Explosion bomb in explosionList)
                {
                    bomb.showBox = false;
                }
            }
        }

    }
}
