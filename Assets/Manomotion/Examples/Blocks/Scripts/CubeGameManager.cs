﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeGameManager : MonoBehaviour
{

    private static CubeGameManager _instance;

    public static CubeGameManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Debug.LogError("More than 1 CubeManagers in the scene");
            Destroy(this.gameObject);
        }
    }
    public string interactableTag = "ExampleBlock";
    private void Start()
    {
        instructions.SetActive(!gameHasStarted);
        cursorRectTransform = cursor.GetComponent<RectTransform>();
        totalPoints = 0;
        streak = 0;

        if (!gameHasStarted)
        {
            gameHasStarted = true;
            instructions.SetActive(!gameHasStarted);
            scoreKeeper.enabled = gameHasStarted;
        }
    }

    int streak;
    public bool gameHasStarted;
    int totalPoints;
    public ManoGestureTrigger interactionTrigger = ManoGestureTrigger.CLICK;
    public ManoClass movingManoclass = ManoClass.PINCH_GESTURE_FAMILY;

    public GameObject cursor;
    RectTransform cursorRectTransform;
    [SerializeField]
    GameObject instructions;
    [SerializeField]
    Text scoreKeeper;

    [SerializeField]
    AudioSource fireSound;


    public void AwardPoints(int points)
    {
        if (totalPoints + points >= 0)
        {
            totalPoints += points;
        }
        else
        {
            totalPoints = 0;
        }
        scoreKeeper.text = "Score: " + totalPoints;
    }



    // Update is called once per frame
    void Update()
    {
        GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
        TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
        Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;

        MoveCursorAt(gesture, trackingInfo, warning);
        //pickup(gesture);
        //FireAt(gesture);

    }

    /// <summary>
    /// Moves the cursor according to the gesture information in the center of the detected bounding box.
    /// The cursor will disapear if there is no hand detected -> Warning Hand not found
    /// </summary>
    /// <param name="gestureInfo">Gesture info.</param>
    /// <param name="trackingInfo">Tracking info.</param>
    /// <param name="warning">Warning.</param>
    void MoveCursorAt(GestureInfo gestureInfo, TrackingInfo trackingInfo, Warning warning)
    {

        if (warning != Warning.WARNING_HAND_NOT_FOUND && gestureInfo.mano_class == movingManoclass)
        {
            if (!cursor.activeInHierarchy)
            {
                cursor.SetActive(true);
            }
            cursorRectTransform.position = Camera.main.ViewportToScreenPoint(trackingInfo.bounding_box_center);
        }
        else
        {
            if (cursor.activeInHierarchy)
            {
                cursor.SetActive(false);
            }
        }


    }

    void pickup(GestureInfo gestureInfo)
    {
        if (gestureInfo.mano_gesture_trigger == ManoGestureTrigger.PICK)
        {

            fireSound.Play();
            if (!gameHasStarted)
            {
                gameHasStarted = true;
                instructions.SetActive(!gameHasStarted);
                scoreKeeper.enabled = gameHasStarted;
            }

            Ray ray = Camera.main.ScreenPointToRay(cursorRectTransform.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.transform.tag == interactableTag)
                {
                    hit.transform.GetComponent<CubeSpawn>().AwardPoints();
                    Handheld.Vibrate();
                    hit.transform.parent = GameObject.Find("CursorGizmo").transform;
                }
            }

            
        }
        
    }



    /// <summary>
    /// Fires a raycast from the position of the cursor forward seeking to hit an example block.
    /// The fire will only happen with the user performes the interaction trigger.
    /// </summary>
    /// <param name="gestureInfo">Gesture info.</param>
    /// <param name="trackingInfo">Tracking info.</param>
    void FireAt(GestureInfo gestureInfo)
    {
        if (gestureInfo.mano_gesture_trigger == interactionTrigger)
        {

            fireSound.Play();
            if (!gameHasStarted)
            {
                gameHasStarted = true;
                instructions.SetActive(!gameHasStarted);
                scoreKeeper.enabled = gameHasStarted;
            }


            Ray ray = Camera.main.ScreenPointToRay(cursorRectTransform.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.transform.tag == interactableTag)
                {
                    hit.transform.GetComponent<CubeSpawn>().AwardPoints();
                    Handheld.Vibrate();

                }
            }


        }
    }









}
