using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosionSet", menuName = "Explosions/Sets/ObstacleSet")]
public class ObstacleSet : ScriptableObject
{
    public string setName = "New ObstacleSet";
    public GameObject[] ObstaclePrefabs;

}
