using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Asset to set the control explanation text in the pause menu

[CreateAssetMenu(fileName = "ControlsText", menuName = "Data Objects/Controls Text")]
public class ControlText : ScriptableObject
{
    public string[] Options;
    [TextArea(2,5)]
    public string[] Explanations;
}
