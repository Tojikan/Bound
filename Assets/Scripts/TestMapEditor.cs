using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lets us test the current level we're working on. Basically sets the explosions to start triggering at the start of the game
namespace BoundEditor
{
    public class TestMapEditor : MonoBehaviour
    {
        public GameObject player;
        public bool ShowBoxes;

        private PlayerController playercontrols;
        private Transform thisTransform;
        private List<Exploder> exploders;

        void Start()
        {
            exploders = new List<Exploder>();
            thisTransform = GetComponent<Transform>();
            playercontrols = player.GetComponent<PlayerController>();
            GetAllExploderComponents();
            StartAllSequences();
            playercontrols.EnableMovement();

        }

        void Update()
        {
            ShowAllBoxes();
        }

        void GetAllExploderComponents()
        {
            foreach (Transform child in thisTransform)
            {
                Exploder exploder = child.GetComponent<Exploder>();
                exploders.Add(exploder);
            }
        }

        void StartAllSequences()
        {
            foreach (Exploder bomb in exploders)
            {
                bomb.BeginSequence();
            }
        }

        void ShowAllBoxes()
        {
            if (ShowBoxes)
            {
                foreach (Exploder bomb in exploders)
                {
                    bomb.showBox = true;
                }
            }
            else
            {
                foreach (Exploder bomb in exploders)
                {
                    bomb.showBox = false;
                }
            }
        }
    }
}
