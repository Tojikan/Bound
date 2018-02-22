using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundEngine
{
    //Timer class to time obstacle triggers and record completion times
    public class Timer : MonoBehaviour
    {
        public static Timer instance = null;                //Makes sure a singleton
        int time;                                           //The current time of the timer
        const float millisecond = 0.01f;                    //Constant for milliseconds
        public bool testing;                                //Used for internal testing during map editing
        const int maxtime = 6000;                           //The maximum time the timer will go up to before going back to 0. 

        //Delegate and event that contains all receivers that listen to the timer
        public delegate void TimerEvents(int eventTime);        
        public static event TimerEvents TimerEventHandler;

        //Make the timer a singleton
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }


        // Use this for initialization
        void Start()
        {
            //initialize time
            time = 0;

            //If we're in testing mode, starts the timer without the need of a game manager instance
            if (testing)
                StartTimer();
        }

        //Start timer
        public void StartTimer()
        {
            StartCoroutine("TimerRoutine");
        }

        //Stop timer
        public void StopTimer()
        {
            StopCoroutine("TimerRoutine");
        }

        //Reset Timer
        public void ResetTimer()
        {
            time = 0;
        }

        //Timer Coroutine
        IEnumerator TimerRoutine()
        {
            //Continuous loop
            while (true)
            {
                //Wait for one millisecond
                yield return new WaitForSeconds(millisecond);

                //Increment our timer by 1.
                time += 1;
                //Call our delegate to trigger any assigned events, if any
                if (TimerEventHandler != null)
                    TimerEventHandler(time);

                //reset time if we've reached maxtime
                if (time > maxtime)
                    ResetTimer();
            }
        }
    }
}
