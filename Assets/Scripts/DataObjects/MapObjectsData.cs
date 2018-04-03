using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundMaps
{
    //Scriptable Object that can hold data on all EventTriggers and Obstacles
    [CreateAssetMenu(fileName = "New MapObjectsData", menuName = "Data Objects/Map Objects Data")]
    public class MapObjectsData : ScriptableObject
    {
        public List<ObstacleData> obstacleData;
        public List<EventTriggerData> eventData;
    }
}
