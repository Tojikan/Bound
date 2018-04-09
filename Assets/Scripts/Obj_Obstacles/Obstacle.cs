using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Inherited base class for all obstacles
namespace BoundEngine
{
    public class Obstacle : MonoBehaviour
    {
        public bool showBox;
        public int triggerTime;                     //Time when obstacle is triggered
        public int loopLength;                      //When the sequence loops back around
        public bool isEnabled;                      //bool check to enable or disable
        private AudioSource audioSource;            //Reference to audiosource for explosion sFx


        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            EnableObstacle();
        }


        //Select Audio player 
        #region AudioSelection
        //Enum to select which Audio player to play from
        public enum Audioplayers
        {
            sFxPlayerOne,
            sFxPlayerTwo,
            sFxPlayerThree,
            sFxPlayerFour,
            sFxPlayerFive
        }
        public Audioplayers SelectSFXPlayer = Audioplayers.sFxPlayerOne;

        //Switches audio players based on which Audio player is selected. Defaults to AudioPlayer one. 
        public void PlayAudio()
        {
            switch ((int)SelectSFXPlayer)
            {
                case 0:
                    SoundManager.instance.AudioPlayerOne(audioSource.clip);
                    break;
                case 1:
                    SoundManager.instance.AudioPlayerTwo(audioSource.clip);
                    break;
                case 2:
                    SoundManager.instance.AudioPlayerThree(audioSource.clip);
                    break;
                case 3:
                    SoundManager.instance.AudioPlayerFour(audioSource.clip);
                    break;
                case 4:
                    SoundManager.instance.AudioPlayerFive(audioSource.clip);
                    break;
                default:
                    SoundManager.instance.AudioPlayerOne(audioSource.clip);
                    break;
            }
        }
        #endregion
        //Constructor classes - has multiple overloads to reflect different obstacle types which might accept different parameters
        #region Constructor classes
        public virtual void ConstructObstacle(int time, int loop){}

        public virtual void ConstructObstacle(int time, int loop, int audio){}

        #endregion
        //Commands called for enabling, disabling, and triggering obstacles and more
        #region Obstacle Commands
        protected virtual void OnDisable()
        {
            Timer.TimerEventHandler -= TriggerObstacle;
        }

        //virtual function for each obstacle. Triggers obstacle at the time. Takes in a parameter of current time
        public virtual void TriggerObstacle(int timerTime)
        {
            if (CheckZero() == false)
                DestroyObstacle();
        }

        //Set the bool and subscribe to the timer event
        public virtual void EnableObstacle()
        {
            Timer.TimerEventHandler += TriggerObstacle;
            isEnabled = true;
        }

        //Remove from timer event and set bool
        protected virtual void DisableObstacle()
        {
            Timer.TimerEventHandler -= TriggerObstacle;
            isEnabled = false;
        }

        //Destroys the obstacle
        public virtual void DestroyObstacle()
        {
            Timer.TimerEventHandler -= TriggerObstacle;
            Destroy(gameObject);
        }

        //Used to draw big red boxes over the area of the collider, useful for editing/creating maps. Simply set to true/false in the editor window or in the explosion container editor
        private void OnDrawGizmos()
        {
            if (showBox == true)
            {
                Gizmos.color = Color.red;
                BoxCollider2D boundary = GetComponent<BoxCollider2D>();
                Gizmos.DrawCube(transform.position, boundary.size);
            }
        }
        #endregion

        //Checks if the length of the loop is zero, which is invalid
        protected virtual bool CheckZero()
        {
            if (loopLength == 0)
            {
                Debug.Log("Invalid loop length.");
                return false;
            }
            return true;
        }
    }
}
