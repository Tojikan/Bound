using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundMaps
{
    [CreateAssetMenu(fileName = "New BombTimers", menuName = "Data Objects/Exploder Data Object")]
    public class ExploderDataObject : ScriptableObject
    {
        public List<ObstacleData> data;
    }
}
