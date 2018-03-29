using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoundMaps;
using BoundEngine;
using System;
using UnityEngine.SceneManagement;


//Game Manager is the controller and handles all of the logic between transitioning levels, calling functions to load maps, checking if game over, etc. 

/** GENERAL GUIDE TO HOW THE GAME PLAYS AND WHICH CLASS HANDLES WHAT 
 * Game Manager will pass back and forth between the Dialogue Manager instance, Transition Manager instance, and player to handle timing at the end of dialogues/transitions. So the game executes through these 4 classes
 * To help you review later, the order of events is as follows
 * Game initializes and loads a map from the currentmap scriptable object located in the Resources/MapData folder. This is loaded into a mapfile variable. At the end of initialize, LoadLevel is called
 * LoadLevel is called which loads the tiles, sets the music (doesn't play it yet), Creates the obstacles using the obstacle manager, starts a level fade, and then tries to start a dialogue
 * If we have a dialogue, then goes to DialogueManager, which will call GameManager LevelTransition at the end of the Dialogue. If no dialogue, skip straight to LevelTransition
 * LevelTransition will go to the transitionmanager instance to play a transition based on if we're at the start or end of a level. If we're at the start, plays the start transition
 * At the end of any transition, the Dialogue Manager will go back to Game Manager depending if we're at the start or end of a level. If start, then it calls StartLevel to enable movement, play music, start obstacles, etc.
 * Then the game plays as normal. Whenever player hits an obstacle, it'll go back to Game Manager to check decrement lives but player handles everything else. If player collides with the finish object then we go back to Game Manager
 * Back in game manager, we'll call level finish to do the level cleanup stuff. Then it goes back to the Dialogue manager for any end game dialogue, which goes back to level transition at the end
 * Once we trigger the end level transition, the transition manager will check if we're at the end of the level and call LoadNextLevel instead. If we're at the end of the map, transition manager will check that and do a map complete animation instead
 * Then we got to map complete. So the general gameplay order is as follows
 * GM -> DM -> GM -> TM -> GM -> Player -> GM -> DM -> TM -> GM
 * Hope that helps
 **/



public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance = null;                   //Make the game manager a singleton
    public PlayerController playControl;                                    //reference to our player controller to set movement 
    public GameObject obstacles;                                            //reference to our obstacle container object;
    public GameObject Player;                                               //Player Game Object
    public GameObject Spawn;                                                //Spawn point
    public GameObject endImage;                                             //reference to our end screen
    public int playerLives = 50;                                            //Int for player lives
    public int levelToLoad = 0;                                             //Set which level we're loading
    public float dialogueDelay = 0.6f;                                      //Delay from level load to displaying dialogue
    public Text endText;                                                    //reference to our end screen text
    public Text livesCounter;                                               //Text to display our lives
    public BoundsInt gameArea;                                              //our game area to play in
    public static bool checkInPlay;                                         //static bool to inform other other classes if in play. Such as pausemenu
    public CurrentMapSelection selectedMap;                                 //Drag reference to the map scriptable object here

    [HideInInspector] public int endLevel;                                  //Last level
    [HideInInspector] public int currentLevel;                              //Current Level

    private bool levelStart;                                                //Bool check for start or end of the level for different transitions/dialogue
    private MapFile currentMap;                                             //Variable to store our map
    private TileSet tileSet;                                                //reference to our tileset
    private RenderMap mapRenderer;                                          //reference to our renderMap Component
    private ObstacleSet obstacleSet;                                        //reference to our explosion set
    private ObstacleManager obstacleManager;                                //reference to our obstacle manager



    #region initial setup
    private void Awake()
    {
        //Check to make sure this is the only instance of Game Manager
        if (GameManagerInstance == null)
            GameManagerInstance = this;
        else if (GameManagerInstance != this)
            Destroy(gameObject);

        //Get our components
        playControl = Player.GetComponent<PlayerController>();
        mapRenderer = GetComponent<RenderMap>();
        obstacleManager = obstacles.GetComponent<ObstacleManager>();

        //initialize bool
        checkInPlay = false;
    }



    //Initial setup of the game
    void InitGame()
    {
        //Set the map
        currentMap = SetMap(selectedMap);

        //gets the number of the last level
        endLevel = (currentMap.numberOfLevels) - 1;

        //initial level set to 0
        currentLevel = 0;

        //Load from File
        try
        {
            tileSet = GetTileSet(currentMap.tileset);
            obstacleSet = GetObstacleSet(currentMap.obstacleSet);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Unable to load map data");
            ExitBackToMenu();
        }

        //Set Text
        livesCounter.text = "Lives: " + playerLives;

        playControl.DisableMovement();

        Debug.Log("Game Initialized");
        //Load level
        LoadLevel(0);
    }

 
    private void Start()
    {
        //Initiate game
        InitGame();
    }

    #endregion

    #region Everything related to loading levels
    //Loads a given level from the current loaded map. Sets up our tiles, places our exploders and then calls our transition
    private void LoadLevel(int level)
    {
        Debug.Log("Loading " + level);
        try
        {
            //Renders the tiles of our current map file
            mapRenderer.LoadTiles(currentMap.levels[level], tileSet, gameArea);
            //set our start/endpoints
            mapRenderer.SetBeacons(currentMap.levels[level].startPoint, currentMap.levels[level].endPoint);
            //Creates our obstacles
            Timer.instance.StopTimer();
            obstacleManager.CreateExploders(currentMap.levels[level].obstacles, obstacleSet);
            //Moves our player to the start point
            SpawnPlayer();
            //Set music from file
            SoundManager.instance.SetMusic(currentMap.levels[level].music);
            //Bool to check if we are at the start of a new level
            levelStart = true;
            TransitionManager.instance.Fade(true);
            Debug.Log("Load Successful");
            //Starts level Dialogue
            Invoke("StartDialogue", 1.0f);
         }

        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Unable to load level. Exiting");
            ExitBackToMenu();
        }
    }


    //Loads the next level.
    public void LoadNextLevel()
    {
        //Checks to see if we're on the last level
        if (currentLevel >= endLevel)
        {
            return;
        }

        //increments currentlevel and loads the next
        currentLevel += 1;
        LoadLevel(currentLevel);
    }


    //Returns a reference to a tileset. Takes in the name of a tileset.
    public TileSet GetTileSet(string path)
    {
        //Sets our tileset path by getting the info from the mapfile and setting a path
        UnityEngine.Object temp = Resources.Load("Tiles/Assets/" + path);
        if (!temp)
            Debug.Log("Tileset could not be loaded");
        return (TileSet)temp;
    }


    //Returns a reference to explosion set. Takes in the name of the set
    public ObstacleSet GetObstacleSet(string path)
    {
        //Sets our tileset path by getting the info from the mapfile and setting a path
        UnityEngine.Object temp = Resources.Load("Obstacles/" + path);
        if (!temp)
            Debug.Log("Explosionset could not be loaded!");
        return (ObstacleSet)temp;
    }

    //Returns the map data from the map scriptable object
    private MapFile SetMap(CurrentMapSelection selection)
    {
        return selection.mapData;
    }

    #endregion

    #region Everything related to level transitions
    //called at the end of the level when player collides with a finish object. Remember that movement gets disabled on collision
    //Also recall the dialogue manager will call back to GameManager at the end of the dialogue to go to the next step
    public void LevelFinish()
    {
        //Stops music and timer and clears the obstacles
        SoundManager.instance.StopMusic();
        Timer.instance.StopTimer();
        obstacleManager.ClearObstacles();

        //Tells StartDialogue to play the end dialogue not the start dialogue
        levelStart = false;
        //For other classes, like pausemenu, to know if we're in play
        checkInPlay = false;
        //start end dialogue
        StartDialogue();
    }


    //Dialogue at the start of a level
    private void StartDialogue()
    {
        Debug.Log("Loading Dialogue");

        // temp dialogue variable
        Dialogue dialogue;
        
        //Check if we are at level start or end and set the dialogue accordingly
        if (!levelStart)
        {
            dialogue = currentMap.levels[currentLevel].endDialogue;
        }
        else
        {
            dialogue = currentMap.levels[currentLevel].startDialogue;
        }

        //If the dialogue is non existent or has no text then just start the level transition animation
        if (dialogue.sentences == null || dialogue.sentences.Length == 0)
        {
            LevelTransition();
            return;
        }
        else
        {
            //Load the dialogue from the current level
            //The DialogueManager calls the next step to the game
            DialogueManager.instance.StartDialogue(dialogue);
        }
    }


    //Begins the level transitions
    public void LevelTransition()
    {
        //If at the start of a level, plays a level start transition
        if (levelStart)
        {
            //This bool tells the transition manager whether we are at the start or end of a level
            TransitionManager.start = true;
            TransitionManager.instance.LevelStart(currentLevel + 1);
        }
        //At the end of a level, plays a level complete transition
        else if (!levelStart)
        {
            //this bool is passed into the TransitionManager LevelEnd to tell it if we are at the end of the map 
            bool mapEnd = false;

            //Checks to see if we're on the last level
            if (currentLevel >= endLevel)
            {
                mapEnd = true;
            }

            //Set the transition manager bool and then calls the levelEnd
            TransitionManager.start = false;
            TransitionManager.instance.LevelEnd(mapEnd);
        }
    }



    //Begins a level
    public void StartLevel()
    {
        //Resets the timer
        Timer.instance.ResetTimer();
        //trigger our obstacles to start
        Timer.instance.StartTimer();
        //Play music
        SoundManager.instance.PlayMusic();
        //Allow movement
        playControl.EnableMovement();
        checkInPlay = true;
        Debug.Log("Level Start");
    }

    //Called when player runs into an obstacle. Sets the game over events or decrements the lives counter
    public bool CheckGameOver()
    {
        //Checks to see if we have lives, then calls Game Over if not
        if (playerLives <= 0)
        {
            GameOver();
            return true;
        }

        //Decrease lives by one. 
        else if (playerLives > 0)
        {
            playerLives -= 1;
            livesCounter.text = "Lives: " + playerLives;
            return false;
        }
        return false;
    }

    //Moves player to the spawn point
    public void SpawnPlayer()
    {
        Player.transform.position = Spawn.transform.position;
    }


    //Displays text upon defeat. Clears obstacles and ends the game manager
    public void GameOver()
    {
        endImage.SetActive(true);
        obstacleManager.ClearObstacles();
        endText.text = "You Lose";
        Destroy(GameManagerInstance);
    }
    #endregion

    //Returns to the loadMap screen
    public void ExitBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

#if UNITY_EDITOR
    //For skipping levels when testing in the editor
    public void LevelSkip()
    {
        SoundManager.instance.StopMusic();
        Timer.instance.StopTimer();
        obstacleManager.ClearObstacles();
    }

#endif

}
