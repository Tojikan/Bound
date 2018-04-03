using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundEngine
{
    //Script for moving the spawn point once player hits a certain trigger
    public class Savepoint : EventTrigger
    {
        private GameObject spawnPoint;                              //Reference to the spawn point object
        public AudioClip soundEffect;                               //Drag desired sound effect played when triggered

        private void Start()
        {
            //Finds our spawn point object
            spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        }

        //Called when collision detected
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Vector2 temp = spawnPoint.transform.position;
                //Switch the spawn points
                spawnPoint.transform.position = this.transform.position;
                transform.position = temp;
                PlaySound();
            }
        }

        //Loads our set clip into the SoundManager to be played
        void PlaySound()
        {
            SoundManager.instance.PlayerSounds(soundEffect);
        }
    }
}
