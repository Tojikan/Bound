using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;
using UnityEngine.UI;

namespace BoundEngine
{
    public class DialogueManager : TouchInput
    {
        public static DialogueManager instance = null;
        public Text nameText;
        public Text dialogueText;
        public GameObject dialogueBox;
        public Dialogue dialogue;
        private bool isTalking;

        private Queue<string> sentences;
        private Queue<string> speakers;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);

            sentences = new Queue<string>();
            speakers = new Queue<string>();
        }

        protected override void Update()
        {
            base.Update();
            if (isTalking)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    DisplayNextSentence();
                }
            }
        }

        protected override void OnTouchBeganAnywhere()
        {
            if (isTalking)
            {
                DisplayNextSentence();
            }
        }

        public void StartDialogue(Dialogue dialogue)
        {
            dialogueBox.SetActive(true);
            sentences.Clear();
            isTalking = true;

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


        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            nameText.text = speakers.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        void EndDialogue()
        {
            dialogueBox.SetActive(false);
            isTalking = false;
            GameManager.GameManagerInstance.LevelStartTransition();

        }



        IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }
    }
}
