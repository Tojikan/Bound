using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundEngine
{
    //manages our sounds and music
    //Plays sounds and music from empty audio sources which are assigned clips when they are called, letting you dynamically assign sound
    //Two dedicated channels for music and player sounds
    //Five channels for other SFX from obstacles, etc. 
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance = null;                         //Lets us call our sound manager from other scripts
        public AudioSource efxSource;                                       //first SFX player
        public AudioSource efxSourceTwo;                                    //second SFX player
        public AudioSource efxSourceThree;                                  //third SFX player
        public AudioSource efxSourceFour;                                   //fourth SFX player
        public AudioSource efxSourceFive;                                   //fifth SFX player
        public AudioSource playerSounds;                                    //Audio dedicated to player sounds
        public AudioSource musicSource;                                     //Audio dedicated to BGM


        private void Awake()
        {
            //Sets us to be a singleton. If the instance doesn't exist, set it to this. Otherwise destroy
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        //First Clip Player
        public void AudioPlayerOne(AudioClip clip)
        {
            //Set clip
            efxSource.clip = clip;

            //If we're playing another, stop
            if (efxSource.isPlaying)
                efxSource.Stop();
            //Then play this
            efxSource.Play();
        }

        //Second Clip Player
        public void AudioPlayerTwo(AudioClip clip)
        {
            //Set clip
            efxSourceTwo.clip = clip;

            //If we're playing another, stop
            if (efxSourceTwo.isPlaying)
                efxSourceTwo.Stop();
            //Then play this
            efxSourceTwo.Play();
        }

        //Third Clip Player
        public void AudioPlayerThree(AudioClip clip)
        {
            //Set clip
            efxSourceThree.clip = clip;

            //If we're playing another, stop
            if (efxSourceThree.isPlaying)
                efxSourceThree.Stop();
            //Then play this
            efxSourceThree.Play();
        }

        //Fourth Clip Player
        public void AudioPlayerFour(AudioClip clip)
        {
            //Set clip
            efxSourceFour.clip = clip;

            //If we're playing another, stop
            if (efxSourceThree.isPlaying)
                efxSourceThree.Stop();
            //Then play this
            efxSourceFour.Play();
        }

        //Fifth Player
        public void AudioPlayerFive(AudioClip clip)
        {
            //Set clip
            efxSourceFive.clip = clip;

            //If we're playing another, stop
            if (efxSourceFive.isPlaying)
                efxSourceFive.Stop();
            //Then play this
            efxSourceFive.Play();
        }


        //PlayerSounds, like death, movement, etc.
        public void PlayerSounds(AudioClip clip)
        {
            //Set clip
            playerSounds.clip = clip;

            //If we're playing another, stop
            if (playerSounds.isPlaying)
                playerSounds.Stop();
            //Then play this
            playerSounds.Play();
        }


        //play and set music.
        public void MusicPlayer(AudioClip music)
        {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.Play();
        }

        //Stop music
        public void StopMusic()
        {
            musicSource.Stop();
        }
    }
}
