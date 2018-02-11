using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoundMaps;
using BoundEngine;


//Game Manager is the controller and handles all of the logic between transitioning levels, calling functions to load maps, checking if game over, etc. 
public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance = null;                   //Make the game manager a singleton
    public PlayerController playControl;                                    //reference to our player controller to set movement 
    public GameObject obstacles;                                            //reference to our obstacle container object;
    public GameObject Player;                                               //Player Game Object
    public GameObject Spawn;                                                //Spawn point
    public GameObject endImage;                                             //reference to our end screen
    public MapPath pathToMap;                                               //reference our map path scriptable object
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
    private TileSet tileSet;                                                //reference to our tileset
    private RenderMap mapRenderer;                                          //reference to our renderMap Component
    private ExplosionSet explosionSet;                                      //reference to our explosion set
    private ObstacleManager obstacleManager;                                //reference to our obstacle manager


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
        obstacleManager = obstacles.GetComponent<ObstacleManager>();

        //livesCounter.text = "Lives: " + playerLives;                              //Set our lives text

        //Initiate game
        InitGame();
    }



    //Initial setup of the game
    void InitGame()
    {
        //Set the map path
        SetMapPath();

        //Loads level one and reads from our file
        currentMap = mapLoad.LoadMap(mapPath);


        Debug.Log("Loading " + mapPath);

        //gets the number of the last level
        endLevel = (currentMap.numberOfLevels) - 1;

        //initial level set to 0
        currentLevel = 0;

        //Load from File
        tileSet = mapLoad.GetTileSet(currentMap.tileset);
        explosionSet = mapLoad.GetExplosionSet(currentMap.explosionSet);

        //Load level
        LoadLevel(0);

        //Set Text
        livesCounter.text = "Lives: " + playerLives;
        //Allow movement after we load our level
        playControl.EnableMovement();
    }

    //Gets the path to the selected map from the mapPath scriptable object
    public void SetMapPath()
    {
        mapPath = pathToMap.mapfilePath;
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

    //Loads a given level from the current loaded map. Sets up our tiles, places our exploders and then calls our transition
    public void LoadLevel(int level)
    {
        //Renders the tiles of our current map file
        mapRenderer.LoadTiles(currentMap.levels[level], tileSet, gameArea);
        //set our start/endpoints
        mapRenderer.SetBeacons(currentMap.levels[level].startPoint, currentMap.levels[level].endPoint);
        //Creates our exploders
        obstacleManager.CreateExploders(currentMap.levels[level].explosions, explosionSet);
        //Starts level transition
        LevelStartTransition();
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
            try { obstacleManager.ClearObstacles(); }
            catch { }
            currentLevel += 1;
            LoadLevel(currentLevel);
            Respawn();
            playControl.EnableMovement();
        }
    }

    //Function for us to set transitions
    //TO DO: A  transition animation of some sort
    //Currently contains methods to start the games, such as telling our exploders to start exploding and enabling movement. 
    public void LevelStartTransition()
    {
        //Moves our player to the start point
        Respawn();
        //trigger our explosions to start
        obstacleManager.StartExplosions();
        //Allow movement
        playControl.EnableMovement();
    }

    //Called when player runs into an obstacle
    public void PlayerDeath()
    {
        //Checks to see if we have lives, then calls Game Over if not
        if (playerLives <= 0)
        {
            playControl.StopMovement();
            playControl.DisableMovement();
            GameOver();
        }

        //Decrease lives by one. Stops movement and call Respawn to move our player
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


    //Displays text upon successful completion. Clears our obstacles and ends the game manager
    void MapComplete()
    {
        endImage.SetActive(true);
        endText.text = "Map Complete";
        obstacleManager.ClearObstacles();
        Destroy(GameManagerInstance);
    }

    //Displays text upon defeat. Clears obstacles and ends the game manager
    public void GameOver()
    {
        endImage.SetActive(true);
        obstacleManager.ClearObstacles();
        endText.text = "You Lose";
        Destroy(GameManagerInstance);
    }

    //Respawns player to the spawn point. 
    public void Respawn()
    {
        Player.transform.position = Spawn.transform.position;
    }
}
