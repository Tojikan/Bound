using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dialogue class. Does not inherit from Monobehavior, so you need to create it like a variable
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
