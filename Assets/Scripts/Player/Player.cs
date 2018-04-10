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
    public AudioClip levelCompleteSound;                                   //Drag the clip that plays when a level is complete
    public AudioClip gameOverSound;                                        //Drag the clip that plays when game over
    private AudioSource deathAudio;                                        //drag our player death audio here
    private PlayerController playerControl;                                //Player controller component 
    private PlayerCollisions[] bodyColliders;                              //List of all body collider components
    public SpriteRenderer spriteRender;                                    //reference to sprite render component
    public Animator animator;                                              //Reference to animator component
    private bool gameOver;                                                 //Checks if the game is over
    private bool isHit;                                                    //bool to only trigger one hit
    private bool finished;                                                 //bool to check if we hit the finish point
    private GameObject spawnPoint;                                         //reference to startpoint object
    private const float delayTime = 0.75f;                                 //delay for invoke methods


    private void Awake()
    {
        spawnPoint = GetStart();
    }

    //Initialize and get comopnenets
    private void Start()
    {
        bodyColliders = FindObjectsOfType<PlayerCollisions>();
        playerControl = GetComponent<PlayerController>();
        deathAudio = GetComponent<AudioSource>();
        gameOver = false;
        finished = false;
        isHit = false;
    }

    //Upon detecting collision from a trigger collider. 
    //Remember to set Rigidbodies on our collision objects
    //NOW REMOVED: COLLIDERS NOW LOCATED ON CHILDREN OBJECTS
    private void OnTriggerEnter2D(Collider2D other)
    {
        ////If it's tagged with finish, stop and prevent movement. Load next level
        //if (other.tag == "Finish")
        //{
        //    SoundManager.instance.StopMusic();
        //    Timer.instance.StopTimer();
        //    SoundManager.instance.PlayerSounds(levelCompleteSound);
        //    playerControl.StopMovement();
        //    playerControl.DisableMovement();
        //    Invoke("LevelisFinished", delayTime);
        //    return;
        //}
        ////Only trigger one hit at a time
        //if (!isHit && other.tag == "Lethal")
        //{
        //    OnObstacleHit();
        //}

    }

    //This is called by children colliders when they detect a collision with tag "Finish"
    public void EndpointCollision()
    {
        //Bool check to make sure this can only be triggered once
        if (finished)
            return;
        else if (!finished)
        {
            finished = true;
        }
        Debug.Log("Hit Finish point");

        //All followup actions upon hitting an end point
        SoundManager.instance.StopMusic();
        Timer.instance.StopTimer();
        SoundManager.instance.PlayerSounds(levelCompleteSound);
        playerControl.StopMovement();
        playerControl.DisableMovement();
        Invoke("LevelisFinished", delayTime);
        return;
    }

    //Called by children colliders when they detect a collision with their associated body part
    public void OnObstacleHit()
    {
        //Set bool
        isHit = true;

        //If we're playing the main game, do a check
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
        //If not, just respawn
        else
        {
            PlayerDeath();
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
            EnableColliders();
            isHit = false;
            StartCoroutine("FlickerSprite");
        }
        else if (gameOver)
        {
            SoundManager.instance.PlayerSounds(gameOverSound);
        }
    }

    //Disables collider, stops and disables all movements, plays sounds, and then sets the death animation trigger
    private void PlayerDeath()
    {
        DisableColliders();
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

    //Finish level
    private void LevelisFinished()
    {
        finished = false;
        GameManager.instance.LevelFinish();
    }

    //Disable all colliders
    private void DisableColliders()
    {
        foreach (PlayerCollisions body in bodyColliders)
        {
            body.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    //Enable all colliders
    private void EnableColliders()
    {
        foreach (PlayerCollisions body in bodyColliders)
        {
            body.gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }

    //returns a reference to the spawnpoint object
    GameObject GetStart()
    {
        GameObject spawn;
        try
        {
            spawn = GameManager.instance.Spawn;
            return spawn;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Unable to get spawn point from Game Manager. Attempting a search");
        }
        spawn = GameObject.FindGameObjectWithTag("SpawnPoint");
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
