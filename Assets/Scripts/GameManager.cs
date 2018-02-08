using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance = null;                   //Make the game manager a singleton
    public LoadMap loadScript;                                              //create a load map instance
    public PlayerController playControl;                                    //reference to our player controller to set movement

    public GameObject Player;                                               //Player Game Object
    public GameObject Spawn;                                                //Spawn point
    public GameObject endImage;                                             //reference to our end screen

    public Text endText;                                                    //reference to our end screen text
    public Text livesCounter;                                              //Text to display our lives

    public int playerLives = 50;                                            //Int for player lives
    
   

    [HideInInspector] public int endLevel;                                  //Last level
    [HideInInspector] public int currentLevel;                              //Current Level

    


    private void Awake()
    {
        //Check to make sure this is the only instance of Game Manager
        if (GameManagerInstance == null)
        {
            GameManagerInstance = this;
        }
        else if (GameManagerInstance != this)
            Destroy(gameObject);

        //Get our load and player controller components
        loadScript = GetComponent<LoadMap>();
        playControl = Player.GetComponent<PlayerController>();

        livesCounter.text = "Lives: " + playerLives;                              //Set our lives text

        //Initiate game
        InitGame();
    }


    //Initial setup of the game
    void InitGame()
    {
        //Loads level one and reads from our file
        loadScript.SetUpLevel();

        //gets the number of the last level
        endLevel = (loadScript.GetEndLevel()-1);

        //set current level to the first
        currentLevel = 0;

        //Allow movement after we load our level
        playControl.EnableMovement();
    }

    //Loads the next level. Called when player collides with an end point trigger collider
    //Movement is disabled on collision. Remember to reset it again. 
    public void LoadNextLevel()
    {
        //Checks to see if we're on the last level
        if (currentLevel >= endLevel)
        {
            MapComplete();
        }

        //If not, load the next level, increment the current level, and allow movement again. 
        else if(currentLevel < endLevel)
        {

            currentLevel += 1;
            loadScript.LoadLevel(currentLevel);
            playControl.EnableMovement();
        }
    }


    public void PlayerDeath()
    {
        //Checks to see if we have lives, then calls Game Over if not
        if (playerLives <= 0)
        {
            playControl.StopMovement();
            playControl.DisableMovement();
            GameOver();
        }

        //Decrease lives by one. Stop movement and call Respawn to move our player
        else
        {
            playControl.StopMovement();
            playControl.DisableMovement();
            playerLives -= 1;
            livesCounter.text = "Lives: " + playerLives;
            Respawn();
            playControl.EnableMovement();
        }
    }


    //Displays text upon successful completion
    void MapComplete()
    {
        endImage.SetActive(true);
        endText.text = "Map Complete";
        enabled = false;
    }

    //Displays text upon defeat
    public void GameOver()
    {
        endImage.SetActive(true);
        endText.text = "Out of Lives";
        enabled = false;
    }

    //Respawns player to the spawn point. 
    public void Respawn()
    {
        Player.transform.position = Spawn.transform.position;
    }
}
