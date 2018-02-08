using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "Obstacle/Explosion")]
public class Explosion : ScriptableObject
{
    public AnimationClip anim;
    public float animLength;
    public Bounds size;

    public void CalculateValues()
    {
        animLength = anim.length;
        size = anim.localBounds;
    }

}
