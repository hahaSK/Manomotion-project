using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PickUpThings : MonoBehaviour
{

    //Which Continuous Gesture I am going to use

    //Happens all the time
    public ManoGestureContinuous contunuOusGestureToMoveBucket;

    public RectTransform trashCanRectTransform;
    public string interactableTag = "ExampleBlock";
    RectTransform cursorRectTransform;
    // Update is called once per frame
    void Update()
    {
        ManoGestureContinuous theGestureThatWillBeDetectedNow = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_continuous;
        TrackingInfo theTrackingInformation = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;

        MoveTrashCanBasedOnContinuousGesture(theGestureThatWillBeDetectedNow, theTrackingInformation);
    }

    void MoveTrashCanBasedOnContinuousGesture(ManoGestureContinuous aContinuousGesture, TrackingInfo handDetectedTrackingInformation)
    {
        if (aContinuousGesture == contunuOusGestureToMoveBucket)
        {
            //trashCanRectTransform.position = Camera.main.ViewportToScreenPoint(handDetectedTrackingInformation.bounding_box_center);
            float width = Screen.width * handDetectedTrackingInformation.bounding_box.width;
            float height = Screen.height * handDetectedTrackingInformation.bounding_box.height;

            float size = Mathf.Min(width, height);

            //trashCanRectTransform.sizeDelta = new Vector2(size, size);


            Ray ray = Camera.main.ScreenPointToRay(cursorRectTransform.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.transform.tag == interactableTag)
                {
                    hit.transform.position = Camera.main.ViewportToScreenPoint(handDetectedTrackingInformation.bounding_box_center);
                    hit.transform.GetComponent<CubeSpawn>().AwardPoints();
                    Handheld.Vibrate();
                    //hit.transform.parent = GameObject.Find("CursorGizmo").transform;
                }
            }

            //Do Something
        }
    }
}