using BoundMenus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**A singleton class that handles all dialog boxes and system messages to player
- Works by opening up various UI elements on demand
- Button clicks are tied to events. Subscribe functions to this event in order to have different responses
- Can open a single button, two button, or loading dialog box
**/
public class DialogSystem : MonoBehaviour
{
    public GameObject dialogWindow;                     //the main panel object that is parent of all message boxes
    public GameObject dialogBox;                        //the box parent container for all message boxes with buttons
    public GameObject doubleButton;                     //drag a reference to the parent object of the two buttons
    public GameObject singleButton;                     //drag a reference to the single button object
    public GameObject loadingDialog;                    //drag a reference to the loading window object
    public Text loadingText;                            //drag a reference to the loading text
    public Text dialogMessage;                          //drag a reference to the text of the dialog box
    public Text singleButtonText;                       //drag reference to single button text
    public Text doubleButtonOneText;                    //drag reference to double left button text
    public Text doubleButtonTwoText;                    //drag reference to double right button text
    private const float waitTime = 0.1f;                //amount of time to wait before changing the loading text
    public static DialogSystem instance;                //singleton instance variable

    //Singleton
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
                Destroy(gameObject);
    }

    //Pressing a button on a dialog box will call a event. Subscribe methods to this event to have responses to buttons
    #region Events and delegates
    //For single button boxes
    public delegate void SingleButton();
    public static event SingleButton SingleButtonPress;
    //For the left button on a double button
    public delegate void DoubleButtonOne();
    public static event DoubleButtonOne DoubleButtonOnePress;
    //For the right button on a double button press
    public delegate void DoubleButtonTwo();
    public static event DoubleButtonTwo DoubleButtonTwoPress;
    #endregion

    //these are actions that get subscribed to the events
    #region Event Actions 

    //Closes all window and sets the panel parent inactive
    private void CloseWindow()
    {
        CloseAllWindows();
        dialogWindow.SetActive(false);
    }


    //Below functions are called whenever the button is pressed. Function container to call the event
    public void SingleOnClick()
    {
        if (SingleButtonPress != null)
            SingleButtonPress();
    }

    public void DoubleOneOnClick()
    {
        if (DoubleButtonOnePress != null)
            DoubleButtonOnePress();
    }

    public void DoubleTwoOnClick()
    {
        if (DoubleButtonTwoPress != null)
            DoubleButtonTwoPress();
    }



    #endregion


    //these are messages that open and set the dialog window
    #region Messaging

    //Open loading text window
    public void OpenLoadingDialog()
    {
        CloseAllWindows();
        //Set text and actives
        dialogWindow.SetActive(true);
        loadingText.text = "Loading...";
        loadingDialog.SetActive(true);
        StartCoroutine("LoadingText");
    }

    //Opens a dialog box with only a single button. Sets the dialog box message and the button text
    public void SingleButtonMessage(string message, string button)
    {
        //close open dialogs
        CloseAllWindows();
        //set panel active
        dialogWindow.SetActive(true);
        //set the text
        dialogMessage.text = message;
        singleButtonText.text = button;
        //add the close window function to event
        SingleButtonPress += CloseWindow;
        //set active
        dialogBox.SetActive(true);
        singleButton.SetActive(true);
    }

    //Open a dialog box with two buttons. 
    public void DoubleButtonMessage(string message, string button1, string button2)
    {
        //close all open dialogs
        CloseAllWindows();
        //set the panel
        dialogWindow.SetActive(true);
        //set the text
        dialogMessage.text = message;
        doubleButtonOneText.text = button1;
        doubleButtonTwoText.text = button2;
        //add the close window event
        SingleButtonPress += CloseWindow;
        //set active
        dialogBox.SetActive(true);
        singleButton.SetActive(true);
    }

    #endregion

    //Close everything
    private void CloseAllWindows()
    {
        //Get the transform of the panel parent
        Transform thisTransform = dialogWindow.GetComponent<Transform>();

        //iterate over each child object and set them active
        foreach (Transform child in thisTransform)
        {
            child.gameObject.SetActive(false);
        }
        //For safety, we stop any existing coroutines and then set the parent inactive
        StopAllCoroutines();
        dialogWindow.SetActive(false);
    }

    //Changes loading text in a loop. Basically just changes the ellipse into a period
    IEnumerator LoadingText()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            loadingText.text = "Loading.  ";
            yield return new WaitForSeconds(waitTime);
            loadingText.text = "Loading...";
        }
    }

    //Clear all subscribed events from the event on Disable
    //We set the null check to prevent errors from calling null events
    private void OnDisable()
    {
        if (SingleButtonPress != null)
        {
            Delegate[] singleList = SingleButtonPress.GetInvocationList();
            Debug.Log(singleList);
            foreach (var d in singleList)
            {
                SingleButtonPress -= (d as SingleButton);
            }
        }

        if (DoubleButtonOnePress != null)
        {
            Delegate[] doubleListOne = DoubleButtonOnePress.GetInvocationList();
            foreach (var d in doubleListOne)
            {
                DoubleButtonOnePress -= (d as DoubleButtonOne);
            }
        }

        if (DoubleButtonTwoPress != null)
        {
            Delegate[] doubleListTwo = DoubleButtonTwoPress.GetInvocationList();
            foreach (var d in doubleListTwo)
            {
                DoubleButtonTwoPress -= (d as DoubleButtonTwo);
            }
        }

        //for safety, lets stop all routines again
        StopAllCoroutines();
    }
}
