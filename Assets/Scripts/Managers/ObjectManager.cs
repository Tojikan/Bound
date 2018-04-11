using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;

namespace BoundEngine
{
 
    //Class to handle the creation and controls of all our obstacles
    public class ObjectManager : MonoBehaviour
    {
        public bool ShowBoxes;                                                     //Draw explosion collision boxes for debugging
        private List<Obstacle> obstacleList;                                       //List of all exploders currently in the level. Use this to control the obstacles
        private List<EventTrigger> triggerList;                                    //List of all Event Triggers currently in the level


        #region Obstacles
        //Method to create our exploders. Takes in parameters of a list of obstacles and the explosion set
        public void CreateObstacles(List<ObstacleData> obstacles, ObstacleSet set)
        {
            //Create a new list
            obstacleList = new List<Obstacle>();

            //Iterate over each explosiondata in the mapfile explosion list
            foreach (ObstacleData data in obstacles)
            {
                //Creates an exploder prefab at the specified position in the file
                GameObject newObstacle = Instantiate(set.obstaclePrefabs[data.obstacleType].gameObject, data.position, transform.rotation);

                //Replace the clone part so we don't have problems with triggering the animator state
                newObstacle.name = newObstacle.name.Replace("(Clone)", "");

                //Get a reference to the exploder component of our newly created prefab
                Obstacle obstacle = newObstacle.GetComponent<Obstacle>();    

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
        #endregion

        #region EventTriggers
        //Now it creates event triggers instead
        public void CreateEventTriggers(List<EventTriggerData> triggers, EventTriggerSet set)
        {
            //Create a new list
            triggerList = new List<EventTrigger>();

            //Iterate over each explosiondata in the mapfile explosion list
            foreach (EventTriggerData data in triggers)
            {
                //Creates an exploder prefab at the specified position in the file
                GameObject newTrigger = Instantiate(set.prefabs[data.type].gameObject, data.position, transform.rotation);

                //Get a reference to the exploder component of our newly created prefab
                EventTrigger eventTrigger = newTrigger.GetComponent<EventTrigger>();

                //Initialize the prefab data
                eventTrigger.ConstructTrigger(data.position, data.enabled);
                //Adds this expoder to our list for controls
                triggerList.Add(eventTrigger);
            }
        }

        //Destroys all of them
        public void ClearTriggers()
        {
            foreach (EventTrigger child in triggerList)
            {
                child.DestroyTrigger();
            }
        }
        #endregion

        //Draws the Gizmos square over each map object
        void ShowAllBoxes()
        {
            //Check if bool is enabled
            if (ShowBoxes)
            {
                foreach (Obstacle obstacle in obstacleList)
                {
                    obstacle.showBox = true;
                }

                foreach (EventTrigger trigger in triggerList)
                {
                    trigger.showBox = true;
                }
            }
            //Make sure the box doesn't show if it's not true
            else
            {
                foreach (Obstacle obstacle in obstacleList)
                {
                    obstacle.showBox = false;
                }

                foreach (EventTrigger trigger in triggerList)
                {
                    trigger.showBox = false;
                }
            }
        }

        //Remove all obstacles and triggers
        public void ClearList()
        {
            ClearObstacles();
            ClearTriggers();
        }

        //For scenarios where there might not be lists of obstacles and objects
        public void SearchAndDestroy()
        {
            Obstacle[] allObstacles = FindObjectsOfType<Obstacle>();
            foreach (Obstacle obstacle in allObstacles)
            {
                DestroyImmediate(obstacle.gameObject);
            }

            EventTrigger[] allEvents = FindObjectsOfType<EventTrigger>();
            foreach (EventTrigger trigger in allEvents)
            {
                DestroyImmediate(trigger.gameObject);
            }
        }

        //For use in the map editor - set all as a child of the appropriate parent container
        public void SetParents(Transform obstacleParent, Transform objectParent)
        {
            foreach (Obstacle obstacle in obstacleList)
            {
                obstacle.transform.parent = obstacleParent;
            }

            foreach (EventTrigger trigger in triggerList)
            {
                trigger.transform.parent = objectParent;
            }

        }

    }
}
