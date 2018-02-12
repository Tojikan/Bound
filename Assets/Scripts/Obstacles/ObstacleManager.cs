using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;

namespace BoundEngine
{

    //Class to handle the creation and controls of all our explosions
    public class ObstacleManager : MonoBehaviour
    {

        private List<ExploderObstacle> explosionList;                                       //List of all exploders currently in the scene. Use this to control the explosions

        //Method to create our exploders. Takes in parameters of a list of explosions and the explosion set
        public void CreateExploders(List<ExplosionData> explosions, ExplosionSet set)
        {
            //Create a new list
            explosionList = new List<ExploderObstacle>();

            //Iterate over each explosiondata in the mapfile explosion list
            foreach (ExplosionData data in explosions)
            {
                //Creates an exploder prefab at the specified position in the file
                GameObject newBomb = Instantiate(set.ExplosionPrefabs[data.explodeType], data.position, transform.rotation);
                //Get a reference to the exploder component of our newly created prefab
                ExploderObstacle exploder = newBomb.GetComponent<ExploderObstacle>();    
                //Initialize the prefab data
                exploder.Initialize(data.explodeTime, data.loopLength);
                //Adds this expoder to our list for controls
                explosionList.Add(exploder);
            }
        }

        //Iterates over each exploder in the list and begins their sequences
        public void StartExplosions()
        {
            foreach(ExploderObstacle exploder in explosionList)
            {
                exploder.BeginSequence();
            }
        }

        //Iterates over each exploder in the list and stops their sequences
        public void StopExplosions()
        {
            foreach (ExploderObstacle exploder in explosionList)
            {
                exploder.StopSequence();
            }
        }

        //Iterates over each exploder in the list and destroys their GameObject, clearing all Obstacles. 
        public void ClearObstacles()
        {
            foreach (ExploderObstacle child in explosionList)
            {
                Destroy(child.gameObject);
            }
        }

    }
}
