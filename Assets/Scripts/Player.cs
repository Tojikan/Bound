using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

//Class to handle all other Player things such as death animation and sounds
public class Player : MonoBehaviour
{
    public GameObject deathAnimation;                                      //reference our deathAnimation prefab
    public GameObject instance;                                            //Semi-Singleton for our death animation animation object
    private AudioSource deathAudio;                                        //drag our player death audio here
    private PlayerController playerControl;                                //Player controller component 
    private CircleCollider2D collide;                                      //reference to our collider
        private Player player;                                                 //reference to player component

    private void Start()
    {
        playerControl = GetComponent<PlayerController>();
        deathAudio = GetComponent<AudioSource>();
        collide = GetComponent<CircleCollider2D>();
    }


    //Calls the Load next level function from our Game Manager
    private void NextLevel()
    {
        GameManager.GameManagerInstance.LoadNextLevel();
    }

    //Upon detecting collision from a trigger collider. 
    //Remember to set Rigidbodies on our collision objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If it's tagged with finish, stop and prevent movement. Load next level
        if (other.tag == "Finish")
        {
            playerControl.StopMovement();
            playerControl.DisableMovement();
            NextLevel();
        }

        //If it's tagged with lethal, call death function
        if (other.tag == "Lethal")
        {
            Death();
        }
    }

    //Called when player hits a lethal obstacle
    private void Death()
    {
        //Disabler colliders temporarily
        collide.enabled = false;
        //Create our death animation/sound
        PlayerDeath();
        //Move player
        GameManager.GameManagerInstance.PlayerDeath();
        //Renable colliders
        collide.enabled = true;
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

        //Plays the death audio
        SoundManager.instance.PlayerSounds(deathAudio.clip);     

    }
}
