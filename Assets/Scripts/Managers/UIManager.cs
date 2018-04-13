using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoundEngine
{
    //Handles all the UI bar functionality, minus the pause button
    //Change text for level title, lives
    //Game Timer to time how long to complete a map
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;                   //To reference the UI manager in other scripts
        public Text levelTitle;                             //Drag reference to the text that holds level title
        public Text timerText;                              //Drag reference to the text that holds timer text
        public Text livesText;                              //Drag reference to lives text
        public int maxMinutes;                              //Maximum number of minutes the timer goes up to, to prevent int overload and problems with text wrapping
        public const int maxSeconds = 59;                   //Highest count the seconds will get to
        public const int maxMilli = 99;                     //Highest count milliseconds will get to
        private bool reachedMax;                            //Check to see if we reached max minutes
        private int seconds;                                //Seconds counter in our timer
        private int milliseconds;                           //Milliseconds counter in our timer
        private int minutes;                                //Minutes counter in our timer
  
        
        //Create and set instance
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            //intialize values
            reachedMax = false;
            seconds = 0;
            milliseconds = 0;
            minutes = 0;
            //Set initial timer text
            SetTimerText();
            //Subscribe to the timer event
            EnableGameTimer();
        }

        //Sets the level text
        public void SetLevelTitle(string title)
        {
            levelTitle.text = title;
        }

        //Start listening to the timer event in Timer
        public void EnableGameTimer()
        {
            Timer.TimerEventHandler += GameTimer;
        }

        //Stop listening to timer event in Timer
        public void StopGameTimer()
        {
            Timer.TimerEventHandler -= GameTimer;
        }

        //Delegate that listens to timer event
        //Everytime run of the Timer loop, we add to our various counter variables
        public void GameTimer(int timerTime)
        {
            //Check if we are at max
            if (!reachedMax)
            {
                //Add millisecond if below max 
                if (milliseconds < maxMilli)
                    milliseconds++;

                //Otherwise set to 0 and add to second
                else if (milliseconds >= maxMilli)
                {
                    milliseconds = 0;

                    //Increment seconds if below max seconds. Otherwise add to minute
                    if (seconds < maxSeconds)
                        seconds++;
                    else if (seconds >= maxSeconds)
                    {
                        seconds = 0;
                        minutes++;
                        //Checks if we reached max minutes. 
                        if (minutes >= maxMinutes)
                        {
                            HitTimerLimit();
                        }
                    }
                }
                //Set the text of timer
                SetTimerText();
            }
            //Redundancy to make sure we unsubscribe from delegate
            else if (reachedMax)
                StopGameTimer();
        }

        //Sets the numbers on the timer text
        private void SetTimerText()
        {
            //Add leading zeros to our counters
            string milliText = milliseconds.ToString("D2");
            string secondsText = seconds.ToString("D2");
            string minutesText = minutes.ToString("D2");
            //Append together in one string
            string timer = minutesText + ":" + secondsText + ":" + milliText;
            //Set text. 
            timerText.text = timer;
        }

        //When we hit the max minutes, sets the bool check so we stop adding and unsubscribes from delegate for more redundancy
        private void HitTimerLimit()
        {
            reachedMax = true;
            StopGameTimer();
            //Nice visual effect to turn the timer red to let player know you've hit the limit
            timerText.color = Color.red;
        }

        //Change lives text
        public void SetLifeText(int lives)
        {
            livesText.text = "Lives: " + lives;
        }

        //Disable your timer if this happens to get destroyed via a scene load or something
        void OnDisable()
        {
            StopGameTimer();          
        }
    }
}
