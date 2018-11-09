using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Tooth : MonoBehaviour
{

//    [SerializeField] private GameObject _teethManager;
//    
//    public bool BadTooth;
//
//    void Awake()
//    {
////        _teethManager = GameObject.Find("TeethManager");
////        Assert.IsNotNull(_teethManager);
//        //_teethManager.GetComponent<TeethManager>().AddTooth(this);
//        //_teethManager.GetComponent<TeethManager>().Teeth.Add(this);
//        
//    }

    public virtual void Interact(GestureInfo gestureInfo,TrackingInfo trackingInformation)
    {
        //Base Method
    }
}
