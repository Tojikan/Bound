using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for Explosion Obstacles
//Functions are called at various points in the explosion's animations as an AnimationEvent
public class ExplosionObstacle : MonoBehaviour
{
    //Get a reference to our collider
    private BoxCollider2D boxCollider;

    //When we're enabled, get the component
    void OnEnable()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    //Destroys this explosion
    void StopExplosion()
    {
        Destroy(gameObject);
    }

    //Enables the collider
    void EnableCollider()
    {
        boxCollider.enabled = true;
    }

    //Disables the collider
    void DisableCollider()
    {
        boxCollider.enabled = false;
    }
}
