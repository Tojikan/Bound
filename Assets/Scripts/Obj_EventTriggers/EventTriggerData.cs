using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Struct to store data on event triggers for usage in saving/loading
namespace BoundMaps
{
    [Serializable]
    public class EventTriggerData
    {
        public Vector2 position;                    //position of each object
        public int type;                            //Object type
        public bool enabled;                        //if enabled at start


        public EventTriggerData(Vector2 position, int type, bool enabled)
        {
            this.position = position;
            this.type = type;
            this.enabled = enabled;
        }
    }
}

