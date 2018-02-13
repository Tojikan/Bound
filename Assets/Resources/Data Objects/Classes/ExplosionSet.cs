using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosionSet", menuName = "Explosions/Sets/ExplosionSet")]
public class ExplosionSet : ScriptableObject
{
    public string setName = "New ExplosionSet";
    public GameObject[] ExplosionPrefabs;

}
