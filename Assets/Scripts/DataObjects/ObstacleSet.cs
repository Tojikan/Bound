using BoundEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Arrays of Obstacles and their animations to crossreference for saving/loading 
[CreateAssetMenu(fileName = "New ObstacleSet", menuName = "Data Objects/Sets/ObstacleSet")]
public class ObstacleSet : ScriptableObject
{
    public string setName = "New ObstacleSet";                      //Set name string. Currently unused
    public Obstacle[] obstaclePrefabs;                              //Stores the prefab
    public Sprite[] obstacleSprites;                                //Stores the sprites of the obstacle in order to match sprites to get array index later for loading/saving
}
