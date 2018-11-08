
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TriggerGizmo : MonoBehaviour
{
    public float fadeSpeed = 2f;

    private float Alpha = 1f;
    private Text triggerLabelText;


    private Vector3 increaseScaleFactor;
    public bool canExpand;

    public Color clickColor, pickColor, dropColor, grabColor, releaseColor, tapColor;


    void OnEnable()
    {
        triggerLabelText = GetComponent<Text>();
        canExpand = true;
        increaseScaleFactor = Vector3.one * 0.01f;
    }

    void FixedUpdate()
    {
        triggerLabelText.enabled = canExpand;
        if (canExpand)
        {
            Alpha = Mathf.Lerp(Alpha, 0f, fadeSpeed * Time.deltaTime * 5);
            Color CurrentColor = triggerLabelText.color;
            triggerLabelText.color = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, Alpha);
            transform.localScale += increaseScaleFactor;

            if (Alpha < 0.05f)
            {
                canExpand = false;

            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public virtual void SetTriggerGizmoVisualization(ManoGestureTrigger triggerGesture)
    {
        if (!triggerLabelText)
        {
            triggerLabelText = GetComponent<Text>();
        }
        switch (triggerGesture)
        {
            case ManoGestureTrigger.CLICK:

                triggerLabelText.text = "Click";
                triggerLabelText.color = clickColor;
                break;
            case ManoGestureTrigger.DROP:
                triggerLabelText.text = "Drop";
                triggerLabelText.color = dropColor;
                break;
            case ManoGestureTrigger.PICK:
                triggerLabelText.text = "Pick";
                triggerLabelText.color = pickColor;
                break;
            case ManoGestureTrigger.GRAB_GESTURE:
                triggerLabelText.text = "Grab";
                triggerLabelText.color = grabColor;
                break;
            case ManoGestureTrigger.RELEASE_GESTURE:
                triggerLabelText.text = "Release";
                triggerLabelText.color = releaseColor;
                break;

            default:
                break;
        }
    }


    //public void SetCustomTriggerGizmoVisualization(ManoGestureTrigger triggerGesture)
    //{
    //    if (!triggerLabelText)
    //    {
    //        triggerLabelText = GetComponent<Text>();
    //    }
    //    switch (triggerGesture)
    //    {

    //        case ManoGestureTrigger.DROP:
    //            triggerLabelText.text = "Drop";
    //            triggerLabelText.color = dropColor;
    //            break;
    //        case ManoGestureTrigger.PICK:
    //            triggerLabelText.text = "Pick";
    //            triggerLabelText.color = pickColor;
    //            break;
    //        case ManoGestureTrigger.GRAB_GESTURE:
    //            triggerLabelText.text = "Grab";
    //            triggerLabelText.color = grabColor;
    //            break;
    //        case ManoGestureTrigger.RELEASE_GESTURE:
    //            triggerLabelText.text = "Release";
    //            triggerLabelText.color = releaseColor;
    //            break;
    //        case ManoGestureTrigger.CLICK:
    //            triggerLabelText.text = "Click";
    //            triggerLabelText.color = clickColor;
    //            break;

    //        default:
    //            break;
    //    }
    //}


}
