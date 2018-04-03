using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;
using System;

//Class to handle all other Player Events such as respawning, death, etc
public class Player : MonoBehaviour
{
    public int flickerLength = 3;                                          //How long the player's sprite flickers after respawning
    public float flickerTime = 1f;                                         //Time between flickers
    private AudioSource deathAudio;                                        //drag our player death audio here
    private PlayerController playerControl;                                //Player controller component 
    private Collider2D collide;                                            //reference to the collider that detects for obstacle collisions
    private SpriteRenderer spriteRender;                                   //reference to sprite render component
    private Player player;                                                 //reference to player component
    private Animator animator;                                             //Reference to animator component
    private bool gameOver;                                                 //Checks if the game is over
    private bool isHit;                                                    //bool to only trigger one hit
    private GameObject spawnPoint;                                         //reference to startpoint object


    private void Awake()
    {
        spawnPoint = GetStart();
    }

    //Initialize and get comopnenets
    private void Start()
    {
        playerControl = GetComponent<PlayerController>();
        deathAudio = GetComponent<AudioSource>();
        collide = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        gameOver = false;
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
                GameManager.instance.LevelFinish();
            }

            //Only trigger one hit at a time
            if (!isHit && other.tag == "Lethal")
            {
                isHit = true;

                if (GameManager.instance != null)
                {
                    //If it's tagged with lethal, call death function
                    if (GameManager.instance.CheckGameOver() == false)
                    {
                        PlayerDeath();
                    }
                    else
                    {
                        gameOver = true;
                        PlayerDeath();
                    }
                }
                else
                {
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
            try
            {
                SpawnPlayer();
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                Debug.Log("Error: Could not respawn character");
            }
            collide.enabled = true;
            isHit = false;
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
        playerControl.joystick.HideJoystick();
        animator.SetTrigger("Death");
    }

    //Spawns player
    public void SpawnPlayer()
    {
        transform.position = spawnPoint.transform.position;
    }

    //returns a reference to the spawnpoint object
    GameObject GetStart()
    {
        GameObject spawn;
        try
        {
            spawn = GameManager.instance.Spawn;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Unable to get spawn point from Game Manager. Attempting a search");
            spawn = GameObject.FindGameObjectWithTag("SpawnPoint");
        }
        return spawn;
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
