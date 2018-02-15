using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

//Class to handle all other Player things such as death animation and sounds
public class Player : MonoBehaviour
{
    public int flickerLength = 3;                                          //How long the player's sprite flickers after respawning
    public float flickerTime = 1f;                                      //Time between flickers
    private AudioSource deathAudio;                                        //drag our player death audio here
    private PlayerController playerControl;                                //Player controller component 
    private CircleCollider2D collide;                                      //reference to our collider
    private SpriteRenderer spriteRender;                                   //reference to sprite render component
    private Player player;                                                 //reference to player component
    private Animator animator;                                             //Reference to animator component
    private bool isHit;

    private void Start()
    {
        playerControl = GetComponent<PlayerController>();
        deathAudio = GetComponent<AudioSource>();
        collide = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
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
                if (!isHit)
                {
                    isHit = true;
                    collide.enabled = false;
                    if (GameManager.GameManagerInstance.CheckGameOver() == false)
                        PlayerDeath();
                }

            }
        }
    }

 



    public void Respawn()
    {
        animator.SetTrigger("Respawn");
        GameManager.GameManagerInstance.SpawnPlayer();
        collide.enabled = true;
        StartCoroutine("FlickerSprite");
    }


    private void PlayerDeath()
    {
        playerControl.StopMovement();
        playerControl.DisableMovement();
        SoundManager.instance.PlayerSounds(deathAudio.clip);
        animator.SetTrigger("Death");
    }

    IEnumerator FlickerSprite()
    {
        spriteRender.enabled = false;
        yield return new WaitForSeconds(flickerTime);
        spriteRender.enabled = true;
        yield return new WaitForSeconds(flickerTime);
        spriteRender.enabled = false;
        yield return new WaitForSeconds(flickerTime);
        spriteRender.enabled = true;
        yield return new WaitForSeconds(flickerTime);
        spriteRender.enabled = false;
        yield return new WaitForSeconds(flickerTime);
        spriteRender.enabled = true;
        Debug.Log("Flickered?");

        playerControl.EnableMovement();
        isHit = false;

    }
}
