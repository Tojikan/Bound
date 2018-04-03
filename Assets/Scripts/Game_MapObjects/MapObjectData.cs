using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundMaps
{
    public class MapObjectData
    {
        public Vector2 position;                    //position of each object
        public int type;                            //Object type
        public bool enabled;                        //if enabled at start


        public MapObjectData(Vector2 position, int type, bool enabled)
        {
            this.position = position;
            this.type = type;
            this.enabled = enabled;
        }
    }
}

