using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;
using UnityEngine.UI;

//Handles game dialogue box and text, which appear as Unity UI elements
//Inherits from touch input so we can skip to next sentence
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
        private bool isTalking;                                                 //Check if we are talking
        private Queue<string> sentences;                                        //Queue for a FIFO format for sentences
        private Queue<string> speakers;                                         //Queue for FIFO for speaker

        private void Awake()
        {
            //Set singleton instance
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);

            //Initiate queues
            sentences = new Queue<string>();
            speakers = new Queue<string>();
        }

        protected override void Update()
        {
            //For touch input 
            base.Update();

#if UNITY_EDITOR
            //Mouse to load next sentence for testing purposes in the editor
            if (isTalking)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    textSound.PlayOneShot(textSound.clip);
                    DisplayNextSentence();
                }
            }
#endif
        }


        //Accept touch input to display next sentence
        protected override void OnTouchBeganAnywhere()
        {
            //On tap. If we're talking, play a sound and load the next sentence
            if (isTalking)
            {
                textSound.PlayOneShot(textSound.clip);
                DisplayNextSentence();
            }
        }

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
            GameManager.GameManagerInstance.LevelTransition();

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
    }
}
