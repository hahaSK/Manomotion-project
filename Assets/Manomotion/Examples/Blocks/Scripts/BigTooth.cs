using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigTooth : Tooth
{
    public ManoGestureTrigger triggerGesture = ManoGestureTrigger.CLICK;
    public ManoGestureContinuous continuousGesture = ManoGestureContinuous.CLOSED_HAND_GESTURE;

    public List<Tooth> Teeth;

    public RectTransform cursorRectTransform;
    public string interactableTag;

    private AudioSource AudioSource;

    public float depth = 5;

    //public bool BadTooth;

    void Start()
    {
        AudioSource = GameObject.Find("Sound").GetComponent<AudioSource>();
    }

    void Update()
    {
        GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
        TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
        Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;

        Interact(gesture, trackingInfo);
    }

    public override void Interact(GestureInfo gestureInfo, TrackingInfo trackingInformation)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("PickingObjects");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.transform.tag == interactableTag)
                {
                    Destroy(hit.transform.gameObject);
                    AudioSource.Play();
                }
                if (hit.transform.tag == "Tongue")
                {
                    AudioSource.Play();
                    StartCoroutine(LickScreen(hit.transform));
                }
            }
//                    Destroy(this.gameObject);
//            print(name);
//            base.DestroyObj();

//            Vector3 boxCenter = Input.mousePosition;//trackingInformation.bounding_box_center;
//            boxCenter.z = depth;
//            this.transform.position = Camera.main.ViewportToWorldPoint(boxCenter);
        }

//        if (gestureInfo.mano_gesture_continuous == continuousGesture)
//        {
//            //Destroy(this.gameObject);
//            //Interact with the Big Tooth using a continuous Gesture
//            Vector3 boxCenter = trackingInformation.bounding_box_center;
//            boxCenter.z = depth;
//            this.transform.position = Camera.main.ViewportToWorldPoint(boxCenter);
//        }

        if (gestureInfo.mano_gesture_trigger == triggerGesture)
        {


            //Interact with the Big Tooth using a triggerGesture Gesture

            Ray ray = Camera.main.ScreenPointToRay(cursorRectTransform.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Handheld.Vibrate();
                if (hit.transform.tag == interactableTag)
                {
                    AudioSource.Play();
                    Destroy(hit.transform.gameObject);
                }
                if (hit.transform.tag == "Tongue")
                {
                    StartCoroutine(LickScreen(hit.transform));
                }
            }

            //            Ray ray = Camera.main.ScreenPointToRay(cursorRectTransform.transform.position);
            //            RaycastHit hit;
            //            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            //            {
            //                if (hit.transform.tag == interactableTag)
            //                {
            //                    Handheld.Vibrate();
            //
            //                    Destroy(hit.transform.gameObject);
            //                    Vector3 boxCenter = trackingInformation.bounding_box_center;
            //                    boxCenter.z = depth;
            //                    this.transform.position = Camera.main.ViewportToWorldPoint(boxCenter);
            //                }
            //            }
        }
    }

    IEnumerator LickScreen(Transform tongue)
    {
        float rate = 1f / 2.5f;
        float t = 1f;

        Quaternion endRotation = tongue.rotation * Quaternion.Euler(-120, 0, 0);

        print(endRotation.eulerAngles);
        while (t > Single.Epsilon)
        {
            t -= Time.deltaTime * rate;
            print(tongue.transform.rotation.eulerAngles);
            tongue.rotation = Quaternion.Slerp(endRotation, tongue.transform.rotation, t);
            yield return null;
        }
    }
}