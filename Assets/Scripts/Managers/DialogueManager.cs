﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Handles game dialogue box and text, which appear as Unity UI elements
//Inherits from touch input so we can skip to next sentence
//Has its own audio source. Volume mute is hardcoded in
namespace BoundEngine
{
    public class DialogueManager : TouchInput
    {
        public static DialogueManager instance = null;                          //Singleton instance
        public Text nameText;                                                   //Reference to the text element for speaker
        public Text dialogueText;                                               //Reference to the text element for dialogue
        public GameObject dialogueBox;                                          //Reference to the container for the dialogue box to hide/show as needed                                          
        public AudioSource textSound;                                           //Play sound when moving to next sentence
        public AudioSource textMusic;                                           //Background Dialogue music
        public float musicVolume = 0.2f;                                        //Music volume
        private bool isTalking;                                                 //Check if we are talking
        private Queue<string> sentences;                                        //Queue for a FIFO format for sentences
        private Queue<string> speakers;                                         //Queue for FIFO for speaker

        protected override void Awake()
        {
            base.Awake();

            //Set singleton instance
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);

            //Initiate queues
            sentences = new Queue<string>();
            speakers = new Queue<string>();

            textMusic.volume = musicVolume;
        }

        protected override void Update()
        {
            base.Update();
            //Mouse to load next sentence for testing purposes
#if UNITY_STANDALONE
            if (isTalking && PauseMenu.isPaused == false)
            {
                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
                {
                    textSound.PlayOneShot(textSound.clip);
                    DisplayNextSentence();
                }
            }
#elif UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE)
            /**if in editor, always accept right clicks in case you're compiling in mobile. 
            That way won't trigger a double click if you happen to be tapping on surface 
            as surface taps register as mousebutton 0**/

            if (isTalking && PauseMenu.isPaused == false)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    textSound.PlayOneShot(textSound.clip);
                    DisplayNextSentence();
                }
            }
#endif
        }

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        //Accept touch input to display next sentence
        protected override void OnTouchBeganAnywhere()
        {

            //On tap. If we're talking, play a sound and load the next sentence
            if (isTalking && PauseMenu.isPaused == false)
            {
                textSound.PlayOneShot(textSound.clip);
                DisplayNextSentence();
            }
        }
#endif
        //Initiates a dialogue
        public void StartDialogue(Dialogue dialogue)
        {
            //Set our dilaogue box active
            dialogueBox.SetActive(true);
            //Clear the queue
            sentences.Clear();
            speakers.Clear();
            //Set talking
            isTalking = true;

            textMusic.PlayOneShot(textMusic.clip);

            /**Queue sentences and speakers**/
            foreach(string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            foreach (string speaker in dialogue.speakerName)
            {
                speakers.Enqueue(speaker);
            }

            DisplayNextSentence();
        }


        //Displays the next sentence into the dialogue
        public void DisplayNextSentence()
        {
            //If no more sentences, end the dialogue
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            //Dequeue a sentence
            string sentence = sentences.Dequeue();
            //Dequeue and set a speaker
            nameText.text = speakers.Dequeue();
            //Start typing the sentence
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        //End of Dialogue when we reached the end of the sentence
        void EndDialogue()
        {
            //Stop music
            textMusic.Stop();
            dialogueBox.SetActive(false);
            isTalking = false;
            //Begin the Level Transition
            GameManager.instance.LevelTransition();

        }


        //Type out each sentence letter by letter
        IEnumerator TypeSentence(string sentence)
        {
            //Initialize to blank
            dialogueText.text = "";

            //Type out each letter then wait out each frame
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }

        //Mutes and unmutes music, used for muting music from pause menu
        public void MuteDialogueMusic()
        {
            if (textMusic.volume == musicVolume)
            {
                textMusic.volume = 0;
            }
            else
            {
                textMusic.volume = musicVolume;
            }

        }
    }
}
