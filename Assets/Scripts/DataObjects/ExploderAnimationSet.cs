using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionAnimations", menuName = "Explosions/Sets/Exploder Animation Set")]
public class ExploderAnimationSet: ScriptableObject
{
    public string setName = "New Explosion Animations";
    public AnimationClip[] ExplosionAnimations;

}

