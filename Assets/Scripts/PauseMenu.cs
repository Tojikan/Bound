using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles the in-game menu and pauses the game
//Works through the Unity UI system and with the EventSystem
public class PauseMenu : MonoBehaviour
{
    public ControlText controlText;                                     //Drag reference to the ControlText scriptable object with our controller text data
    public static bool isPaused;                                        //Static bool to check if we are paused
    public PlayerController playerControl;                              //Reference to the player's player controller object
    public Text controlOptionName;                                      //Drag reference to the text that will display the current controls
    public Text controlOptionText;                                      //Drag reference to the text that display the control explanation
    private AudioSource[] allAudio;                                     //Audiosource array to pause all sounds being played when paused
    private int controlTypes;                                           //Length of the control options enum
    private int currentControl;                                         //The current control that is set

    private void Awake()
    {
        //Get the length of the control enum
        controlTypes = GetControlTypes();
    }

    //Called when enabled by the pause button
    private void OnEnable()
    {
        //Set time to zero
        Time.timeScale = 0f;

        //Populate the audiosource array with all audio sources in the game and pause
        allAudio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        PauseAudio();

        //Get the current control mode and set the text
        currentControl = (int)PlayerController.controlOptions;
        SetControlsText();

        //Disable movement and set pause bool
        playerControl.DisableMovement();
        isPaused = true;
    }

    #region Resume and Quit
    //Called by the resume button when touched
    public void Resume()
    {
        isPaused = false;
        
        //Set time back to normal
        Time.timeScale = 1f;

        //Resume audio
        ResumeAudio();

        //Enable movement
        playerControl.EnableMovement();

        //Disable the pause menu
        gameObject.SetActive(false);
    }
    //TO DO: Open up a confirmation window when touched and then quits back to the main menu
    public void Quit()
    {
        Debug.Log("You hit quit!");
    }



    #endregion

    #region Audio Settings


    //Pauses all audio sources by iterating through the audiosource array and pausing
    void PauseAudio()
    {
        foreach (AudioSource audio in allAudio)
        {
            audio.Pause();
        }
    }
   

    //Unpauses the audio. Don't use audio.play unless you want to destroy your ears. 
    void ResumeAudio()
    {
        foreach (AudioSource audio in allAudio)
        {
            audio.UnPause();
        }
    }
    #endregion

    #region For setting controls
    //Right button to increment the current control
    public void NextControlRight()
    {
        //If we're at the end of the enum length, go back to 0
        if (currentControl >= (controlTypes - 1))
        {
            currentControl = 0;
        }
        else
        {
            currentControl++;
        }

        //Set the control and change the text
        PlayerController.controlOptions = (PlayerController.ControlOptions)currentControl;
        SetControlsText();
    }

    //Left button to decrement the current control
    public void NextControlLeft()
    {
        //If we're at zero or below, cycle back through the enum
        if (currentControl <= 0)
        {
            currentControl = controlTypes - 1;
        }
        else
        {
            currentControl--;
        }

        //Set the controls and change the text
        PlayerController.controlOptions = (PlayerController.ControlOptions)currentControl;
        SetControlsText();
    }

    //Set the control mode name and explanation text in the pause menu
    void SetControlsText()
    {
        controlOptionName.text = "CONTROL MODE: " + controlText.Options[currentControl];
        controlOptionText.text = controlText.Explanations[currentControl];
    }

    //Gets the length of the control options enum 
    int GetControlTypes()
    {
        return System.Enum.GetNames(typeof(PlayerController.ControlOptions)).Length;
    }
    #endregion

}
