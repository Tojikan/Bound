using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoundMenus
{
    public class MainMenuFunctions : MonoBehaviour
    {
        public void ExitToMain()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void MapSelectScreen()
        {
            SceneManager.LoadScene("LoadMap");
        }

        public void OptionsMenu()
        {
            Debug.Log("Options Menu here");
            return;
        }
    }
}
