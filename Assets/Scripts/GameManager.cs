using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoundMaps;
using BoundEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance = null;                   //Make the game manager a singleton
    public PlayerController playControl;                                    //reference to our player controller to set movement 
    public GameObject Player;                                               //Player Game Object
    public GameObject Spawn;                                                //Spawn point
    public GameObject endImage;                                             //reference to our end screen
    public int playerLives = 50;                                            //Int for player lives
    public int levelToLoad = 0;                                             //Set which level we're loading
    
    public Text endText;                                                    //reference to our end screen text
    public Text livesCounter;                                               //Text to display our lives
    public BoundsInt gameArea;                                              //our game area to play in


    public string mapPath;                                                  //Path to the map we're trying to load. Set in the Editor Window
    [HideInInspector] public int endLevel;                                  //Last level
    [HideInInspector] public int currentLevel;                              //Current Level

    private MapLoader mapLoad;                                              //reference to our MapLoader
    private MapFile currentMap;                                             //Variable to store our map
    private TileSet tileSet;                                                //Variable to reference our tileset
    private RenderMap mapRenderer;                                          //reference to our renderMap Component

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
        mapLoad = GetComponent<MapLoader>();
        playControl = Player.GetComponent<PlayerController>();
        mapRenderer = GetComponent<RenderMap>();

        //livesCounter.text = "Lives: " + playerLives;                              //Set our lives text

        //Initiate game
        InitGame();
    }



    //Initial setup of the game
    void InitGame()
    {
        try
        {
            //Loads level one and reads from our file
            currentMap = mapLoad.LoadMap(mapPath);
        }
        catch
        {
            Debug.Log("Unable to load map. Exiting...");
            return;
        }

        Debug.Log(currentMap.numberOfLevels);
        //gets the number of the last level
        endLevel = (currentMap.numberOfLevels) - 1;

        //initial level set to 0
        currentLevel = 0;

        tileSet = mapLoad.GetTileSet(currentMap.tileset);

        LoadLevel(0);

        //Allow movement after we load our level
        playControl.EnableMovement();
    }

    //Checks if the current level exceeds total levels
    public bool CheckLevelNum()
    {

        if (currentLevel > endLevel)
        {
            return false;
        }
        return true;
    }

    //Loads a new map
    public void LoadLevel(int level)
    {        
        mapRenderer.LoadTiles(currentMap.levels[level], tileSet, gameArea);
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
            LoadLevel(currentLevel);
            Respawn();
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
        else if (playerLives > 0)
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
        Destroy(GameManagerInstance);
    }

    //Displays text upon defeat
    public void GameOver()
    {
        endImage.SetActive(true);
        endText.text = "Out of Lives";
        Destroy(GameManagerInstance);
    }

    //Respawns player to the spawn point. 
    public void Respawn()
    {
        Player.transform.position = Spawn.transform.position;
    }
}
