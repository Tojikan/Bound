using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

//Class to handle all other Player things such as death animation and sounds
public class Player : MonoBehaviour
{
    public int flickerLength = 3;                                          //How long the player's sprite flickers after respawning
    public float flickerTime = 1f;                                         //Time between flickers
    private AudioSource deathAudio;                                        //drag our player death audio here
    private PlayerController playerControl;                                //Player controller component 
    private CircleCollider2D collide;                                      //reference to our collider
    private SpriteRenderer spriteRender;                                   //reference to sprite render component
    private Player player;                                                 //reference to player component
    private Animator animator;                                             //Reference to animator component
    private bool gameOver;                                                 //Checks if the game is over


    //Initialize and get comopnenets
    private void Start()
    {
        playerControl = GetComponent<PlayerController>();
        deathAudio = GetComponent<AudioSource>();
        collide = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        gameOver = false;
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
                if (GameManager.GameManagerInstance.CheckGameOver() == false)
                    PlayerDeath();
                else
                {
                    gameOver = true;
                    PlayerDeath();
                }
            }
        }
    }

 
    //If game isn't over, moves player back to start, re-enables collider, and starts the flicker routine
    //Called at the end of the death animation via animation event
    public void Respawn()
    {
        if (!gameOver)
        {
            animator.SetTrigger("Respawn");
            GameManager.GameManagerInstance.SpawnPlayer();
            collide.enabled = true;
            StartCoroutine("FlickerSprite");
        }
    }

    //Disables collider, stops and disables all movements, plays sounds, and then sets the death animation trigger
    private void PlayerDeath()
    {
        collide.enabled = false;
        playerControl.StopMovement();
        playerControl.DisableMovement();
        SoundManager.instance.PlayerSounds(deathAudio.clip);
        animator.SetTrigger("Death");
    }

    //Coroutine to flicker our player sprite a few times when respawned. At the end, reenables movement. 
    IEnumerator FlickerSprite()
    {
        int x = 0;
        while (x < flickerLength)
        {
            spriteRender.enabled = false;
            yield return new WaitForSeconds(flickerTime);
            spriteRender.enabled = true;
            yield return new WaitForSeconds(flickerTime);
            x++;
        }
        playerControl.EnableMovement();
    }
}
