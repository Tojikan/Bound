using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lets us test the current level we're working on. Basically sets the explosions to start triggering at the start of the game
//Also lets us draw red boxes over the explosions
namespace BoundEditor
{
    public class TestMapEditor : MonoBehaviour
    {
        public GameObject player;                           //drag the player game object to this
        public bool ShowBoxes;                              //Bool to set the Unity Gizmos to draw a red box over our exploder game objects

        private PlayerController playercontrols;            //Reference to our player controls from the dragged player game object
        private Transform thisTransform;                    //Reference to this transform
        private List<ExploderObstacle> exploders;           //List of all explodderobstacles that are a child of the game object of this script

        void Start()
        {
            exploders = new List<ExploderObstacle>();                           //new exploder list 
            thisTransform = GetComponent<Transform>();                          //Get this transform
            playercontrols = player.GetComponent<PlayerController>();           //Get the player controller of our player object
            GetAllExploderComponents();                                         //Grab our exploder scripts
            StartAllSequences();                                                //Set sequences to start
            playercontrols.EnableMovement();                                    //Enable movement

        }

        //Shows boxes if the gizmo bool is set
        void Update()
        {
            ShowAllBoxes();
        }

        //Gets the exploder script reference from all child game objects
        void GetAllExploderComponents()
        {
            //Iterate over each transform under this transform
            foreach (Transform child in thisTransform)
            {
                //Get the script and add it to our list
                ExploderObstacle exploder = child.GetComponent<ExploderObstacle>();
                exploders.Add(exploder);
            }
        }

        //Iterates over the exploder list to begin the timing sequence
        void StartAllSequences()
        {
            foreach (ExploderObstacle bomb in exploders)
            {
                bomb.BeginSequence();
            }
        }

        //Draws the Gizmos square over each exploder
        void ShowAllBoxes()
        {
            //Check if bool is enabled
            if (ShowBoxes)
            {
                foreach (ExploderObstacle bomb in exploders)
                {
                    bomb.showBox = true;
                }
            }
            //Make sure the box doesn't show if it's not true
            else
            {
                foreach (ExploderObstacle bomb in exploders)
                {
                    bomb.showBox = false;
                }
            }
        }
    }
}
