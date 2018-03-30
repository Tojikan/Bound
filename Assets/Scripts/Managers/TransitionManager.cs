using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BoundEngine
{
    //Manages level and game transitions
    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager instance = null;                        //Singleton instance
        public float levelDelay = 0.5f;                                         //Time between loading a new level and end fade out
        public float lowPitchRange = 0.85f;                                     //low range to vary sound effect pitch
        public float highPitchRange = 1.15f;                                    //high range to vary sound effect pitch
        public Text screenText;                                                 //Reference to the Transition screen text
        public AudioSource startSound;                                          //Audio source to play at the start of transition
        public AudioSource endSound;                                            //Audio source to play at the end of transition
        public static bool start;                                               //Static bool to determine if we are at the start of a level or end
        private Animator animator;                                              //Reference to animator component
        private bool reachedEnd;                                                //Checks if you reached end of map


        // Use this for initialization
        void Awake()
        {
            //Make singleton
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            //Get animator comopnent
            animator = GetComponent<Animator>();
            reachedEnd = false;
        }

        //Level start transition when a new level begins
        public void LevelStart(int level)
        {
            //Set text and create a line break between the level and number
            string text = "Level" + '$' + level;
            text = text.Replace('$', '\n');
            screenText.text = text;
            //Trigger animation
            animator.SetTrigger("LevelTransition");
        }


        //Fades the screen in and out. Takes in a bool and triggers a different animations depending on the bool
        public void Fade(bool fadeIn)
        {
            Debug.Log("Fading");
            if (fadeIn)
            {
                animator.SetTrigger("FadeIn");
            }
            else
            {
                animator.SetTrigger("FadeOut");
            }
        }

        //Exit animation at the end of the game, which will exit to the main menu
        public void ExitFade()
        {
            animator.SetTrigger("Exit");
        }

        //Called at the end of a transition animation via animation event
        //Calls back to Game Manager instance to start the level or load the next level depending on the static bool, which is set in Game Manager
        public void TransitionEnd()
        {
            //If at the end, exit back to menu
            if (reachedEnd)
            {
                ExitFade();
            }

            //start the level
            if (start)
            {
                StartPlaying();
            }
            //load the  next level
            else if (!start)
            {
                Fade(false);
                Invoke("NextLevel", levelDelay);
            }
        }

        //Called by Game manager if player hits a level end collider
        //Accepts a bool parameter in order to determine if we finished the map or not
        public void LevelEnd(bool mapFinished)
        {
            //MapComplete animation if we finished the map
            if (mapFinished)
            {
                MapComplete();
            }
            //Otherwise level complete
            else
            {
                string text = "Level    Complete!";
                screenText.text = text;
                animator.SetTrigger("LevelTransition");
            }
        }

        //Sets the text to Map Complete
        //TO DO: A SEPERATE MAP COMPLETE ANIMATION AND ART
        public void MapComplete()
        {
            string text = "Map    Complete!";
            screenText.text = text;
            reachedEnd = true;
            animator.SetTrigger("LevelTransition");
        }

        //Calls back to GameManager to exit
        private void ExitBackToMenu()
        {
            GameManager.GameManagerInstance.ExitBackToMenu();
        }

        //Calls back to GameManager to start the current level
        public void StartPlaying()
        {
            GameManager.GameManagerInstance.StartLevel();
        }

        //Calls back to GameManager to load the next level
        public void NextLevel()
        {
            GameManager.GameManagerInstance.LoadNextLevel();
        }

        //Play the start sound for the animation at a random pitch. Called in an animation event
        public void PlayStartSound()
        {
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            startSound.pitch = randomPitch;
            startSound.PlayOneShot(startSound.clip);
        }

        //Play the end sound for the animation at a random pitch. Called in an animation event
        public void PlayEndSound()
        {
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            endSound.pitch = randomPitch;
            endSound.PlayOneShot(endSound.clip);
        }
    }
}