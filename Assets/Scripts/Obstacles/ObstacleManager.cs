using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;

namespace BoundEngine
{
 
    //Class to handle the creation and controls of all our explosions
    public class ObstacleManager : MonoBehaviour
    {
        public bool ShowBoxes;                                                       //Draw explosion collision boxes for debugging
        private List<Explosion> explosionList;                                       //List of all exploders currently in the scene. Use this to control the explosions

        //Method to create our exploders. Takes in parameters of a list of explosions and the explosion set
        public void CreateExploders(List<ExplosionData> explosions, ExplosionSet set)
        {
            //Create a new list
            explosionList = new List<Explosion>();

            //Iterate over each explosiondata in the mapfile explosion list
            foreach (ExplosionData data in explosions)
            {
                //Creates an exploder prefab at the specified position in the file
                GameObject newBomb = Instantiate(set.ExplosionPrefabs[data.explodeType], data.position, transform.rotation);

                //Get a reference to the exploder component of our newly created prefab
                Explosion exploder = newBomb.GetComponent<Explosion>();    

                //Initialize the prefab data
                exploder.InitializeExplosion(data.explodeTime, data.loopLength, data.audioPlayer);
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
