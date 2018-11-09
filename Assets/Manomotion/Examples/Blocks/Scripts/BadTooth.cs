using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class BadTooth : MonoBehaviour
{
    public ManoGestureTrigger triggerGesture = ManoGestureTrigger.CLICK;

    public RectTransform cursorRectTransform;
    public string interactableTag;

    private GameObject _upperHead;

    //public GameObject GameOverText;

    void Start()
    {
        _upperHead = GameObject.Find("UpperHead");
        Assert.IsNotNull(_upperHead);
    }

    void Update()
    {
        GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
        TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
        Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;

        Interact(gesture, trackingInfo);
    }

    public void Interact(GestureInfo gestureInfo, TrackingInfo trackingInformation)
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.transform.tag == interactableTag)
                {
                    StartCoroutine(closeMouth());
                }
            }
        }

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
                    StartCoroutine(closeMouth());
                }
            }
        }
    }

    IEnumerator closeMouth()
    {
        float rate = 1f / 2.5f;
        float t = 1f;

        Quaternion endRotation = _upperHead.transform.rotation * Quaternion.Euler(-90, 0, 0);

        print(endRotation.eulerAngles);
        while (t > Single.Epsilon)
        {
            t -= Time.deltaTime * rate;
            print(_upperHead.transform.rotation.eulerAngles);
            _upperHead.transform.rotation = Quaternion.Slerp(endRotation, _upperHead.transform.rotation, t);
            yield return null;
        }

        SceneManager.LoadScene("PickingObjects");
    }
}