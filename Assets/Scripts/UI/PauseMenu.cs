using BoundEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Handles the in-game menu and pauses the game
//Works through the Unity UI system and with the EventSystem
public class PauseMenu : MonoBehaviour
{
    public ControlText controlText;                                     //Drag reference to the ControlText scriptable object with our controller text data
    public static bool isPaused;                                        //Static bool to check if we are paused. Used by the pause button to determine if we should pause or not
    public PlayerController playerControl;                              //Reference to the player's player controller object
    public Text controlOptionName;                                      //Drag reference to the text that will display the current controls
    public Text controlOptionText;                                      //Drag reference to the text that display the control explanation
    public Text muteMusicText;                                          //Drag reference to the mute music text
    public Text muteSFXText;                                            //Drag reference to the mute SFX text
    public AudioClip menuOpen;                                          //Drag reference to what sound plays when menu opens
    public AudioClip buttonPress;                                       //Drag reference to what sound plays when button is played
    private int controlTypes;                                           //Length of the control options enum
    private int currentControl;                                         //The current control that is set
    private bool musicIsMute;                                           //Check if music is muted to change text
    private bool sfxIsMute;                                             //Check if sFx is muted to change text
    private AudioSource menuSounds;                                     //AudioSource Component to play sounds


    private void Awake()
    {
        //Get the length of the control enum
        controlTypes = GetControlTypes();
        //Set mute
        musicIsMute = false;
        sfxIsMute = false;
        //Get AudioSource component
        menuSounds = GetComponent<AudioSource>();
        //So our menusounds don't get muted when the menu pauses. If you make a mute all button, must account for this. 
        menuSounds.ignoreListenerPause = true;

        isPaused = false;
    }

    //Called when enabled by the pause button
    private void OnEnable()
    {
        //Play menu open sound
        MenuSounds(menuOpen);
        //Set time to zero
        Time.timeScale = 0f;

        //Pause Audio
        PauseAllAudio();

        //Get the current control mode and set the text
        currentControl = (int)PlayerController.controlOptions;
        SetControlsText();
        SetMuteText();

        //Checks if we're in play and then disables movement if so.
        if (GameManager.checkInPlay == true)
        {
            playerControl.DisableMovement();
        }

        //Set pause bool
        isPaused = true;
    }

    //When disabled, unpauses game and reset time and audio
    //Is also used when you exit a game
    private void OnDisable()
    {
        isPaused = false;
        Time.timeScale = GameManager.gameSpeed;
        ResumeAudio();
    }

    #region Resume and Quit
    //Called by the resume button when touched
    public void Resume()
    {
        //Play button sound
        MenuSounds(buttonPress);

        //Enable movement only if we're in play
        if (GameManager.checkInPlay == true)
        {
            playerControl.EnableMovement();
        }

        //Disable the pause menu
        gameObject.SetActive(false);
    }


    //TO DO: Open up a confirmation window when touched and then quits back to the main menu
    //On pressing the quit button
    public void Quit()
    {
        //Play button sound
        MenuSounds(buttonPress);
        //Resets Game Time so we can actually exit the game
        Time.timeScale = 1.0f;
        //Stops playing all sounds and then unpauses audio
        SoundManager.instance.StopSounds();
        //Exit fade transition
        TransitionManager.instance.ExitFade();
    }

    #endregion


    #region Audio Settings
    //Mutes the music
    public void MusicMuteUnmute()
    {
        //Play button sound
        MenuSounds(buttonPress);

        //Change mute check
        if (musicIsMute)
        {
            musicIsMute = false;
        }
        else if (!musicIsMute)
        {
            musicIsMute = true;
        }

        //Play button sound
        MenuSounds(buttonPress);
        //SoundManager instance to mute music
        SoundManager.instance.MuteMusic();
        //Change text
        SetMuteText();
    }

    //Mutes the SFX
    public void SFXMuteUnmute()
    {
        //Play button sound
        MenuSounds(buttonPress);

        //Change mute check
        if (sfxIsMute)
        {
            sfxIsMute = false;
        }
        else if (!sfxIsMute)
        {
            sfxIsMute = true;
        }

        //SoundManager instance to mute SFX
        SoundManager.instance.MuteSFX();
        //Change text
        SetMuteText();
    }

    //Set the mute button texts
    void SetMuteText()
    {
        if (musicIsMute == true)
        {
            muteMusicText.text = "UNMUTE MUSIC";
        }
        else if (musicIsMute == false)
        {
            muteMusicText.text = "MUTE MUSIC";
        }

        if (sfxIsMute == true)
        {
            muteSFXText.text = "UNMUTE SFX";
        }
        else if (sfxIsMute == false)
        {
            muteSFXText.text = "MUTE SFX";
        }
    }


    //Pauses all audio sources by iterating through the audiosource array and pausing
    void PauseAllAudio()
    {
        AudioListener.pause = true;
    }
   

    //Unpauses the audio. Don't use audio.play unless you want to destroy your ears. 
    void ResumeAudio()
    {
        AudioListener.pause = false;
    }
    #endregion

    #region For setting controls
    //Right button to increment the current control
    public void NextControlRight()
    {
        //Play button sound
        MenuSounds(buttonPress);

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
        //Play button sound
        MenuSounds(buttonPress);

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

    //function to play a menu sounds. Enter clip to play in the parameter. 
    private void MenuSounds(AudioClip clip)
    {
        menuSounds.clip = clip;
        //If we're playing another, stop
        if (menuSounds.isPlaying)
            menuSounds.Stop();
        //Then play this
        menuSounds.Play();
    }
}
