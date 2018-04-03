using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundEngine
{
    public class Savepoint : MapObject
    {
        private GameObject spawnPoint;
        public AudioClip soundEffect;

        private void Start()
        {
            spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            Debug.Log(spawnPoint);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            spawnPoint.transform.position = this.transform.position;
            PlaySound();
            Destroy(this.gameObject);
        }

        void PlaySound()
        {
            SoundManager.instance.PlayerSounds(soundEffect);
        }
    }
}
