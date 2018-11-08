using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGizmos : MonoBehaviour
{


    GizmoManager _gizmoManager;

    private void Start()
    {
        _gizmoManager = GetComponent<GizmoManager>();
    }

    /// <summary>
    /// Toggles the boolean value of showing the hand states
    /// </summary>
    public void ToggleShowHandStates()
    {
        _gizmoManager.Show_hand_states = !_gizmoManager.Show_hand_states;
    }

    /// <summary>
    /// Toggles the boolean value of showing the manoclass
    /// </summary>
    public void ToggleShowManoclass()
    {
        _gizmoManager.Show_mano_class = !_gizmoManager.Show_mano_class;
    }

    /// <summary>
    /// Toggles the boolean value of showing the cursor that follows the bounding box center;
    /// </summary>
    public void ToggleShowCursor()
    {
        _gizmoManager.Show_cursor = !_gizmoManager.Show_cursor;
    }

    /// <summary>
    /// Toggles the boolean value of showing the handside of the detected hand;
    /// </summary>
    public void ToggleShowHandSide()
    {
        _gizmoManager.Show_hand_side = !_gizmoManager.Show_hand_side;
    }


    /// <summary>
    /// Toggles the boolean value of showing the continuous gesture of the detected hand;
    /// </summary>
    public void ToggleShowContinuousGestures()
    {
        _gizmoManager.Show_continuous_gestures = !_gizmoManager.Show_continuous_gestures;
    }

    /// <summary>
    /// Toggles the boolean value of showing Pick Trigger Gesture
    /// </summary>
    public void ToggleShowPickTriggerGesture()
    {
        _gizmoManager.DisplayPick = !_gizmoManager.DisplayPick;
    }

    /// <summary>
    /// Toggles the boolean value of showing Drop Trigger Gesture
    /// </summary>
    public void ToggleShowDropTriggerGesture()
    {
        _gizmoManager.DisplayDrop = !_gizmoManager.DisplayDrop;

    }
    /// <summary>
    /// Toggles the boolean value of showing Click Trigger Gesture
    /// </summary>
    public void ToggleShowClickTriggerGesture()
    {
        _gizmoManager.DisplayClick = !_gizmoManager.DisplayClick;

    }
    /// <summary>
    /// Toggles the boolean value of showing Grab Trigger Gesture
    /// </summary>
    public void ToggleShowGrabTriggerGesture()
    {
        _gizmoManager.DisplayGrab = !_gizmoManager.DisplayGrab;

    }
    /// <summary>
    /// Toggles the boolean value of showing Release Trigger Gesture
    /// </summary>
    public void ToggleShowReleaseTriggerGesture()
    {
        _gizmoManager.DisplayRelease = !_gizmoManager.DisplayRelease;

    }
    /// <summary>
    /// Toggles the show smoothing slider condition.
    /// </summary>
    public void ToggleShowSmoothingSlider()
    {

        _gizmoManager.DisplaySmoothingSliderControler = !_gizmoManager.DisplaySmoothingSliderControler;
        _gizmoManager.DisplaySliderForSmoothing(_gizmoManager.DisplaySmoothingSliderControler);
    }
    /// <summary>
    /// Toggles the show warnings condition.
    /// </summary>
    public void ToggleShowWarnings()
    {
        _gizmoManager.Show_warnings = !_gizmoManager.Show_warnings;
    }
}
