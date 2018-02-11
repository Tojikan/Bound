using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

//Base class for Explosion Obstacles
//Functions are called at various points in the explosion's animations as an AnimationEvent
public class ExplosionObstacle : MonoBehaviour
{
    //Determines which clip player we use
    public bool audioPlayer;

    //Get a reference to our collider
    private BoxCollider2D boxCollider;
    private AudioSource audioSource;

    //When we're enabled, get the component
    void OnEnable()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        if (!audioPlayer)
            SoundManager.instance.PlaySingleTwo(audioSource.clip);
        else
            SoundManager.instance.PlaySingle(audioSource.clip);
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
