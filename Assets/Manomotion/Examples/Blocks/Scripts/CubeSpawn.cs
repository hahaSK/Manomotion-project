using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CubeSpawn : MonoBehaviour
{

    public enum CybeType
    {
        Blue = 0,
        Green = 1,
        Orange = 2,
        Purple = 3,
        Red = 4,
        Yellow = 5
    }

    public Rigidbody rigidbody;
    public MeshRenderer meshRenderer;
    int pointsWorth;
    float timeToDie;
    public Material[] colorMaterials;

    // Use this for initialization
    void OnEnable()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        meshRenderer = this.GetComponent<MeshRenderer>();

    }



    public void AwardPoints()
    {
        CubeGameManager.Instance.AwardPoints(pointsWorth);

        //Destroy(this.gameObject);
    }

    public void Randomize()
    {
        float difficultyTier = 1.5f;
        int maxValue = Enum.GetValues(typeof(CybeType)).Length;
        int minValue = 0;
        int pointsInflation = 5;

        int randomType = UnityEngine.Random.Range(minValue, maxValue);


        meshRenderer.material = colorMaterials[randomType];

        //The red Cube will take points away from you
        if (randomType != (int)CybeType.Red)
        {
            pointsWorth = randomType * pointsInflation;
            timeToDie = maxValue * Mathf.Max(1, difficultyTier) - randomType;
        }
        else
        {
            pointsWorth = -5;
            timeToDie = 2;
        }
        float minXForce = -50;
        float maxXForce = 50;
        float yForce = 50;
        float zForce = 400;

        rigidbody.AddForce(new Vector3(UnityEngine.Random.Range(minXForce, maxXForce), yForce, zForce), ForceMode.Force);
        //Destroy(this.gameObject, timeToDie);

    }
}
