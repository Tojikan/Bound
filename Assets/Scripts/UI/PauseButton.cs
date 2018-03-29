using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pause button OnClick event
public class PauseButton : MonoBehaviour
{
    public GameObject pauseMenu;                            //Reference to the pause menu parent game object            

    //Sets the pausemenu active
    public void PauseGame()
    {
        if (PauseMenu.isPaused == false)
        {
            pauseMenu.SetActive(true);
            PauseMenu.isPaused = true;
        }
    }
}
