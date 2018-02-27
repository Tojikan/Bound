using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundMaps
{
    [System.Serializable]
    public class Dialogue
    {
        public string[] speakerName;
        [TextArea(2, 5)]
        public string[] sentences;
    }
}
