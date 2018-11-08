using UnityEngine;
using UnityEngine.UI;

public class GizmoManager : MonoBehaviour
{

    #region Singleton
    private static GizmoManager _instance;
    public static GizmoManager Instance
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
    #endregion
    [SerializeField]
    Image[] stateImages;

    [SerializeField]
    GameObject handStatesGizmo;
    [SerializeField]
    GameObject manoClassGizmo;
    [SerializeField]
    GameObject handSideGizmo;
    [SerializeField]
    GameObject continuousGestureGizmo;
    [SerializeField]
    GameObject triggerTextPrefab;
    [SerializeField]
    GameObject cursorGizmo;
    [SerializeField]
    GameObject flagHolderGizmo;
    [SerializeField]
    GameObject smoothingSliderControler;
    [SerializeField]
    Text currentSmoothingValue;


    public Color disabledStateColor;
    [SerializeField]
    private bool _show_hand_states, _show_mano_class, _show_trigger_gesture, _show_cursor, _show_hand_side, _show_continuous_gestures, _show_volume_gizmo, _show_warnings, displayPick, displayDrop, displayClick, displayGrab, displayRelease, displaySmoothingSliderControler = true;

    GameObject topFlag, leftFlag, rightFlag;

    RectTransform cursorRectTransform, cursorFillRectTransform;

    Text manoClassText, handSideText, continuousGestureText;




    #region Properties

    public bool Show_volume_gizmo
    {
        get
        {
            return _show_volume_gizmo;
        }

        set
        {
            _show_volume_gizmo = value;
        }
    }


    public bool Show_continuous_gestures
    {
        get
        {
            return _show_continuous_gestures;
        }

        set
        {
            _show_continuous_gestures = value;
        }
    }

    public bool Show_mano_class
    {
        get
        {
            return _show_mano_class;
        }

        set
        {
            _show_mano_class = value;
        }
    }

    public bool Show_trigger_gesture
    {
        get
        {
            return _show_trigger_gesture;
        }

        set
        {
            _show_trigger_gesture = value;
        }
    }

    public bool Show_cursor
    {
        get
        {
            return _show_cursor;
        }

        set
        {
            _show_cursor = value;
        }
    }

    public bool Show_hand_side
    {
        get
        {
            return _show_hand_side;
        }

        set
        {
            _show_hand_side = value;
        }
    }

    public bool Show_hand_states
    {
        get
        {
            return _show_hand_states;
        }

        set
        {
            _show_hand_states = value;
        }
    }

    public bool Show_warnings
    {
        get
        {
            return _show_warnings;
        }

        set
        {
            _show_warnings = value;
        }
    }

    public bool DisplayPick
    {
        get
        {
            return displayPick;
        }

        set
        {
            displayPick = value;
        }
    }

    public bool DisplayDrop
    {
        get
        {
            return displayDrop;
        }

        set
        {
            displayDrop = value;
        }
    }

    public bool DisplayClick
    {
        get
        {
            return displayClick;
        }

        set
        {
            displayClick = value;
        }
    }

    public bool DisplayGrab
    {
        get
        {
            return displayGrab;
        }

        set
        {
            displayGrab = value;
        }
    }

    public bool DisplayRelease
    {
        get
        {
            return displayRelease;
        }

        set
        {
            displayRelease = value;
        }
    }

    public bool DisplaySmoothingSliderControler
    {
        get
        {
            return displaySmoothingSliderControler;
        }

        set
        {
            displaySmoothingSliderControler = value;
        }
    }

    #endregion

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        SetGestureDescriptionParts();
        HighlightStatesToStateDetection(0);
        InitializeFlagParts();

    }

    void Update()
    {
        GestureInfo gestureInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
        TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
        Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;
        Session session = ManomotionManager.Instance.Manomotion_Session;

        DisplayContinuousGestures(gestureInfo.mano_gesture_continuous);
        DisplayManoclass(gestureInfo.mano_class);
        DisplayTriggerGesture(gestureInfo.mano_gesture_trigger, trackingInfo.bounding_box);
        DisplayHandState(gestureInfo.state);
        DisplayCursor(trackingInfo.bounding_box_center, gestureInfo, warning);
        DisplayHandSide(gestureInfo.hand_side);
        DisplayApproachingToEdgeFlags(warning);
        DisplayCurrentsmoothingValue(session);


    }

    #region Display Methods

    /// <summary>
    /// Displays in text value the current smoothing value of the session
    /// </summary>
    /// <param name="session">Session.</param>
    void DisplayCurrentsmoothingValue(Session session)
    {
        if (smoothingSliderControler.activeInHierarchy)
        {
            currentSmoothingValue.text = "Smoothing: " + session.smoothing_controller.ToString("F2");

        }
    }


    /// <summary>
    /// Displayes rough estimation of depth
    /// </summary>
    /// <param name="boundingBoxCenter">Requires the estimated position of the bounding box center.</param>
    void DisplayCursor(Vector3 boundingBoxCenter, GestureInfo gesture, Warning warning)
    {
        if (Show_cursor)
        {
            if (warning != Warning.WARNING_HAND_NOT_FOUND)
            {
                if (!cursorGizmo.activeInHierarchy)
                {
                    cursorGizmo.SetActive(true);
                }
                cursorRectTransform.position = Camera.main.ViewportToScreenPoint(boundingBoxCenter);
                float newFillAmmount = 1 - ((int)(gesture.state / 6) * 0.25f);
                cursorFillRectTransform.localScale = Vector3.Lerp(cursorFillRectTransform.localScale, Vector3.one * newFillAmmount, 0.9f);
            }
            else
            {
                if (cursorGizmo.activeInHierarchy)
                {
                    cursorGizmo.SetActive(false);
                }
            }

        }
        else
        {

            if (cursorGizmo.activeInHierarchy)
            {
                cursorGizmo.SetActive(false);
            }
        }
    }


    /// <summary>
    /// Displays information regarding the detected manoclass
    /// </summary>
    /// <param name="manoclass">Manoclass.</param>
    void DisplayManoclass(ManoClass manoclass)
    {
        manoClassGizmo.SetActive(Show_mano_class);
        if (Show_mano_class)
        {
            switch (manoclass)
            {
                case ManoClass.NO_HAND:
                    manoClassText.text = "Manoclass: No Hand";
                    break;
                case ManoClass.GRAB_GESTURE_FAMILY:
                    manoClassText.text = "Manoclass: Grab Class";
                    break;
                case ManoClass.PINCH_GESTURE_FAMILY:
                    manoClassText.text = "Manoclass: Pinch Class";
                    break;
                case ManoClass.POINTER_GESTURE_FAMILY:
                    manoClassText.text = "Manoclass: Pointer Class";
                    break;
                default:
                    manoClassText.text = "Manoclass: ";
                    break;
            }

        }

    }

    /// <summary>
    /// Displays information regarding the detected manoclass
    /// </summary>
    /// <param name="manoGestureContinuous">Requires a continuous Gesture.</param>
    void DisplayContinuousGestures(ManoGestureContinuous manoGestureContinuous)
    {
        continuousGestureGizmo.SetActive(Show_continuous_gestures);
        if (Show_continuous_gestures)
        {
            switch (manoGestureContinuous)
            {
                case ManoGestureContinuous.CLOSED_HAND_GESTURE:
                    continuousGestureText.text = "Continuous:Closed Hand";
                    break;
                case ManoGestureContinuous.OPEN_HAND_GESTURE:
                    continuousGestureText.text = "Continuous:Open Hand";
                    break;
                case ManoGestureContinuous.HOLD_GESTURE:
                    continuousGestureText.text = "Continuous:Hold";
                    break;
                case ManoGestureContinuous.OPEN_PINCH_GESTURE:
                    continuousGestureText.text = "Continuous:Open Pinch";
                    break;
                case ManoGestureContinuous.POINTER_GESTURE:
                    continuousGestureText.text = "Continuous:Pointing";
                    break;
                case ManoGestureContinuous.NO_GESTURE:
                    continuousGestureText.text = "Continuous:None";
                    break;
                default:
                    continuousGestureText.text = "Continuous:None";
                    break;
            }
        }
    }

    /// <summary>
    /// Displaies the hand side.
    /// </summary>
    /// <param name="handside">Requires a ManoMotion Handside.</param>
    void DisplayHandSide(HandSide handside)
    {
        handSideGizmo.SetActive(Show_hand_side);
        if (Show_hand_side)
        {


            switch (handside)
            {
                case HandSide.Palmside:
                    handSideText.text = "Handside: Palm Side";
                    break;
                case HandSide.Backside:
                    handSideText.text = "Handside: Back  Side";
                    break;
                case HandSide.None:
                    handSideText.text = "Handside: None";
                    break;
                default:
                    handSideText.text = "Handside: None";
                    break;
            }




        }

    }

    ///// <summary>
    ///// Updates the visual information that showcases the hand state (how open/closed) it is
    ///// </summary>
    ///// <param name="gesture_info"></param>
    void DisplayHandState(int handstate)
    {

        handStatesGizmo.SetActive(Show_hand_states);
        if (Show_hand_states)
        {
            HighlightStatesToStateDetection(handstate);
        }

    }

    ManoGestureTrigger previousTrigger;
    /// <summary>
    /// Display Visual information of the detected trigger gesture.
    /// In the case where a click is intended (Open pinch, Closed Pinch, Open Pinch) we are clearing out the visual information that are generated from the pick/drop
    /// </summary>
    /// <param name="triggerGesture">Requires an input of trigger gesture.</param>
    void DisplayTriggerGesture(ManoGestureTrigger triggerGesture, BoundingBox bounding_box)
    {


        if (triggerGesture != ManoGestureTrigger.NO_GESTURE)
        {

            if (displayPick)
            {
                if (triggerGesture == ManoGestureTrigger.PICK)
                {
                    TriggerDisplay(bounding_box, ManoGestureTrigger.PICK);

                }
            }
            if (displayDrop)
            {
                if (triggerGesture == ManoGestureTrigger.DROP)
                {
                    if (previousTrigger != ManoGestureTrigger.CLICK)
                    {
                        TriggerDisplay(bounding_box, ManoGestureTrigger.DROP);
                    }
                }
            }
            if (displayClick)
            {
                if (triggerGesture == ManoGestureTrigger.CLICK)
                {
                    TriggerDisplay(bounding_box, ManoGestureTrigger.CLICK);
                    if (GameObject.Find("PICK"))
                    {
                        Destroy(GameObject.Find("PICK"));
                    }

                }
            }
            if (displayGrab)
            {
                if (triggerGesture == ManoGestureTrigger.GRAB_GESTURE)
                    TriggerDisplay(bounding_box, ManoGestureTrigger.GRAB_GESTURE);
            }
            if (displayRelease)
            {
                if (triggerGesture == ManoGestureTrigger.RELEASE_GESTURE)
                    TriggerDisplay(bounding_box, ManoGestureTrigger.RELEASE_GESTURE);
            }

        }
        previousTrigger = triggerGesture;

    }

    /// <summary>
    /// Displays the visual information of the performed trigger gesture.
    /// </summary>
    /// <param name="bounding_box">Bounding box.</param>
    /// <param name="triggerGesture">Trigger gesture.</param>
    void TriggerDisplay(BoundingBox bounding_box, ManoGestureTrigger triggerGesture)
    {
        GameObject triggerVisualInformation = Instantiate(triggerTextPrefab);
        triggerVisualInformation.name = triggerGesture.ToString();
        triggerVisualInformation.GetComponent<TriggerGizmo>().SetTriggerGizmoVisualization(triggerGesture);
        triggerVisualInformation.transform.SetParent(transform);
        triggerVisualInformation.transform.localPosition = new Vector2((bounding_box.top_left.x + bounding_box.width / 2) * Screen.width, (1 - bounding_box.top_left.y - bounding_box.height / 2) * Screen.height);

        float midX = (bounding_box.top_left.x + bounding_box.width / 2);
        float midY = (1 - bounding_box.top_left.y - bounding_box.height / 2);
        triggerVisualInformation.GetComponent<RectTransform>().position = Camera.main.ViewportToScreenPoint(new Vector2(midX, midY));

    }


    /// <summary>
    /// Visualizes the current hand state by coloring white the images up to that value and turning grey the rest
    /// </summary>
    /// <param name="stateValue">Requires a hand state value to assign the colors accordingly </param>
    void HighlightStatesToStateDetection(int stateValue)
    {
        for (int i = 0; i < stateImages.Length; i++)
        {
            if (i > stateValue)
            {
                stateImages[i].color = disabledStateColor;
            }
            else
            {
                stateImages[i].color = Color.white;
            }
        }
    }

    /// <summary>
    /// Highlights the edges of the screen according to the warning given by the ManoMotion Manager
    /// </summary>
    /// <param name="warning">Requires a warning.</param>
    void DisplayApproachingToEdgeFlags(Warning warning)
    {
        if (_show_warnings)
        {
            if (!flagHolderGizmo.activeInHierarchy)
            {
                flagHolderGizmo.SetActive(true);
            }

            rightFlag.SetActive(warning == Warning.WARNING_APPROACHING_RIGHT_EDGE);
            topFlag.SetActive(warning == Warning.WARNING_APPROACHING_UPPER_EDGE);
            leftFlag.SetActive(warning == Warning.WARNING_APPROACHING_LEFT_EDGE);

        }
        else
        {
            if (flagHolderGizmo.activeInHierarchy)
            {
                flagHolderGizmo.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Displayes the smoothing slider.
    /// </summary>
    /// <param name="display">If set to <c>true</c> display.</param>
    public void DisplaySliderForSmoothing(bool display)
    {
        smoothingSliderControler.SetActive(display);
    }

    /// <summary>
    /// Initializes the components of the Manoclass,Continuous Gesture and Trigger Gesture Gizmos
    /// </summary>
    void SetGestureDescriptionParts()
    {
        manoClassText = manoClassGizmo.transform.Find("Description").GetComponent<Text>();
        handSideText = handSideGizmo.transform.Find("Description").GetComponent<Text>();
        continuousGestureText = continuousGestureGizmo.transform.Find("Description").GetComponent<Text>();
        cursorRectTransform = cursorGizmo.GetComponent<RectTransform>();
        cursorFillRectTransform = cursorGizmo.transform.GetChild(0).GetComponent<RectTransform>();
    }


    /// <summary>
    /// Initializes the components for the visual illustration of warnings related to approaching edges flags.
    /// </summary>
    void InitializeFlagParts()
    {
        topFlag = flagHolderGizmo.transform.Find("Top").gameObject;
        rightFlag = flagHolderGizmo.transform.Find("Right").gameObject;
        leftFlag = flagHolderGizmo.transform.Find("Left").gameObject;
    }

    #endregion




}
