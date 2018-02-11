using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

//Class to handle all other Player things such as death animation and sounds
public class Player : MonoBehaviour
{
    public GameObject deathAnimation;                                      //reference our deathAnimation prefab

    public GameObject instance;
    private AudioSource deathAudio;                                        //reference to our death Audio Source
    private void Start()
    {
        deathAudio = GetComponent<AudioSource>();
    }

    //Creates a player death animation and then plays the death sound
    public void PlayerDeath()
    {

        //Prevents multiple death animations being spawned if you're hitting multiple colliders
        //At the same time, created a new bug where if you hit an explosion before an animation finishes, a death animation won't spawn. 
        //TO DO fix this
        if (instance == null)
          {
             instance = Instantiate(deathAnimation, transform.position, transform.rotation);
          }

        //Spawn a prefab that does a death animation. 
        //TO DO refine further. Maybe build it into the player animation? It caused some bugs but might be better to do that. 
        SoundManager.instance.PlaySingleThree(deathAudio.clip);     

    }
}
