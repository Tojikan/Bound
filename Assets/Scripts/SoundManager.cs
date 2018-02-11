using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundEngine
{
    //manages our sounds and music
    //Can play music and simultaeneously play two clips at the same time
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance = null;                         //Lets us call our sound manager from other scripts
        public AudioSource efxSource;                                       //effect to be played
        public AudioSource efxSourceTwo;                                    //second effect to be played
        public AudioSource efxSourceThree;                                  //third effect to be played
        public AudioSource musicSource;                                     //music to be played


        private void Awake()
        {
            //Sets us to be a singleton. If the instance doesn't exist, set it to this. Otherwise destroy
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        //Plays a single clip at a time. 
        public void PlaySingle(AudioClip clip)
        {
            //Set clip
            efxSource.clip = clip;

            //If we're playing another, stop
            if (efxSource.isPlaying)
                efxSource.Stop();
            //Then play this
            efxSource.Play();
        }

        //Second single clip player. Plays another clip
        public void PlaySingleTwo(AudioClip clip)
        {
            //Set clip
            efxSourceTwo.clip = clip;

            //If we're playing another, stop
            if (efxSourceTwo.isPlaying)
                efxSourceTwo.Stop();
            //Then play this
            efxSourceTwo.Play();
        }
        //Third single clip player. Plays another clip
        public void PlaySingleThree(AudioClip clip)
        {
            //Set clip
            efxSourceThree.clip = clip;

            //If we're playing another, stop
            if (efxSourceThree.isPlaying)
                efxSourceThree.Stop();
            //Then play this
            efxSourceThree.Play();
        }

        //play and set music.
        public void PlayMusic(AudioClip music)
        {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
