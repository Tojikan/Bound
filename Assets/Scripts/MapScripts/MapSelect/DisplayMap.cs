using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;
using UnityEngine.UI;
using System;

namespace BoundMenus
{
    //Displays the map information 
    public class DisplayMap : MonoBehaviour
    {
        public Text mapTitle;                       //Drag reference to the map title text
        public Text mapDescrip;                     //Drag reference to the map description text
        public Image mapImage;                      //Drag reference to the map image
        public Sprite defaultImage;                 //Drag reference to the default image sprite

        private void Start()
        {
            //Set default
            mapImage.sprite = defaultImage;
        }

        //Public function to receive the meta object
        public void SetMapDisplay(MapMetaObject meta)
        {
            SetTitle(meta.name);
            SetDescrip(meta.description);
            SetImage(meta.imagePath);
        }

        //Sets title
        private void SetTitle(string title)
        {
            //Checks if there is a title
            if (title != null || title.Length > 0)
            {
                mapTitle.text = title;
            }
            else
            {
                mapTitle.text = "Untitled";
            }
        }

        //Set description
        private void SetDescrip(string descrip)
        {
            //Checks if there is a description
            if (descrip != null || descrip.Length > 0)
            {
                mapDescrip.text = descrip;
            }
            else
            {
                mapDescrip.text = "No Description.";
            }
        }

        //Sets the map image
        private void SetImage(string path)
        {
            try
            {
                //Tries to load image
                Sprite newImage = Resources.Load(path) as Sprite;
                mapImage.sprite = newImage;
            }
            catch (Exception e)
            {
                //Otherwise, load a default image
                Debug.Log(e);
                Debug.Log("Unable to load map image");
                mapImage.sprite = defaultImage;
            }
        }
    }
}
