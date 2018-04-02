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
        private List<Obstacle> obstacleList;                                       //List of all exploders currently in the scene. Use this to control the obstacles

        //Method to create our exploders. Takes in parameters of a list of obstacles and the explosion set
        public void CreateExploders(List<ObstacleData> explosions, ObstacleSet set)
        {
            //Create a new list
            obstacleList = new List<Obstacle>();

            //Iterate over each explosiondata in the mapfile explosion list
            foreach (ObstacleData data in explosions)
            {
                //Creates an exploder prefab at the specified position in the file
                GameObject newBomb = Instantiate(set.ObstaclePrefabs[data.obstacleType], data.position, transform.rotation);

                //Get a reference to the exploder component of our newly created prefab
                Obstacle obstacle = newBomb.GetComponent<Obstacle>();    

                //Initialize the prefab data
                obstacle.ConstructObstacle(data.triggerTime, data.loopLength, data.audioPlayer);
                //Adds this expoder to our list for controls
                obstacleList.Add(obstacle);
            }
        }

        //Iterates over each exploder in the list and destroys their GameObject, clearing all Obstacles. 
        public void ClearObstacles()
        {
            foreach (Obstacle child in obstacleList)
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
                foreach (Obstacle obstacle in obstacleList)
                {
                    obstacle.showBox = true;
                }
            }
            //Make sure the box doesn't show if it's not true
            else
            {
                foreach (Obstacle obstacle in obstacleList)
                {
                    obstacle.showBox = false;
                }
            }
        }

    }
}
