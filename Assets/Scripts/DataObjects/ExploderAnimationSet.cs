using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//DEPRECATED
//Animation array to crossreference explosions to
[CreateAssetMenu(fileName = "ExplosionAnimations", menuName = "Obstacle Data/Sets/Explosion Animations Set")]
public class ExploderAnimationSet: ScriptableObject
{
    public string setName = "New Explosion Animations";
    public AnimationClip[] ExplosionAnimations;

}

