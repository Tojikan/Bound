using BoundEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Arrays of Obstacles and their animations to crossreference for saving/loading 
[CreateAssetMenu(fileName = "New ObstacleSet", menuName = "Data Objects/Sets/ObstacleSet")]
public class ObstacleSet : ScriptableObject
{
    public string setName = "New ObstacleSet";
    public Obstacle[] obstaclePrefabs;
    public AnimationClip[] animations;
}
