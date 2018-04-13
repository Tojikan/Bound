using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

//Set of EventTriggers and their sprites to crossreference them for loading/saving
[CreateAssetMenu(fileName = "New EventTriggerSet", menuName = "Data Objects/Sets/EventTrigger Set")]
public class EventTriggerSet : ScriptableObject
{
    public string setName = "New EventTriggerSet";
    public EventTrigger[] prefabs;
    public Sprite[] sprites;
}

