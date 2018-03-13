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
        public MusicList musicPlaylist;                                     //List of game songs
        public AudioSource efxSource;                                       //first SFX player
        public AudioSource efxSourceTwo;                                    //second SFX player
        public AudioSource efxSourceThree;                                  //third SFX player
        public AudioSource efxSourceFour;                                   //fourth SFX player
        public AudioSource efxSourceFive;                                   //fifth SFX player
        public AudioSource playerSounds;                                    //Audio dedicated to player sounds
        public AudioSource musicSource;                                     //Audio dedicated to BGM
        private float musicVolume = 0.6f;                                   //volume to play music at
        private AudioSource[] sFXSources;                                   //array of SFX sources

        private void Awake()
        {
            //Sets us to be a singleton. If the instance doesn't exist, set it to this. Otherwise destroy
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            //Set music volume
            musicSource.volume = musicVolume;

            //Initialize and fill SFX array to iterate over
            sFXSources = new AudioSource[5];
            FillAudioSourceArray(sFXSources);
        }

        
        //Lazy bit of hardcoding here. Fix in the future if you come up with a better idea on how to do this.
        void FillAudioSourceArray(AudioSource[] array)
        {
            array[0] = efxSource;
            array[1] = efxSourceTwo;
            array[2] = efxSourceThree;
            array[3] = efxSourceFour;
            array[4] = efxSourceFive;
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

        //Mutes and unmutes music
        public void MuteMusic()
        {
            DialogueManager.instance.MuteDialogueMusic();

            if (musicSource.volume != 0)
            {
                musicSource.volume = 0;
            }

            else if (musicSource.volume == 0)
            {
                musicSource.volume = musicVolume;
            }
        }

        //Mutes and unmutes SFX
        public void MuteSFX()
        {
            foreach (AudioSource source in sFXSources)
            {
                if (source.volume != 0)
                {
                    source.volume = 0;
                }

                else if (source.volume == 0)
                {
                    source.volume = musicVolume;
                }
            }
        }

        
        //set music.
        public void SetMusic(int index)
        {
            musicSource.clip = musicPlaylist.musicPlaylist[index];
            musicSource.loop = true;
        }

        //Stop music
        public void StopMusic()
        {
            musicSource.Stop();
        }

        //play music
        public void PlayMusic()
        {
            musicSource.Play();
        }
    }
}
