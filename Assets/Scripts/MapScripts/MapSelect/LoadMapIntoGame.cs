using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BoundEngine;
using System;

namespace BoundMenus
{
    //Button event to load map
    public class LoadMapIntoGame : MonoBehaviour
    {
        public CurrentMapSelection mapContainer;                            //Drag reference to map selection object
        private MapLoader mapLoader;                                        //MapLoader component

        private void Awake()
        {
            mapLoader = GetComponent<MapLoader>();
        }

        //This is the OnClick function event that will load and start the game
        public void StartButtonOnClick()
        {
            try
            {
                //Set the target map data by loading the map from the file using loadmap
                mapContainer.mapData = mapLoader.LoadMap(mapContainer.meta.fileLocation);
                StartGame();
            }
            catch (Exception e)
            {
                //TO DO: ERROR MESSAGE SYSTEM
                Debug.LogException(e);
                Debug.Log("unable to load map " + mapContainer.meta.fileLocation);
            }
        }

        //Starts loading the main game scene
        private void StartGame()
        {
            StartCoroutine(LoadSceneAsync());
        }

        //Asynchronous loading coroutine
        //Loads the next scene in the background without interrupting
        IEnumerator LoadSceneAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Prototype");

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
