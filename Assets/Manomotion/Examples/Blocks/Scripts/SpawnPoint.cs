using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float timeToSpawn;
    public float spawnDelay = 3f;
    public GameObject cubePrefab;

    void Start()
    {
        SpawnCubes();
    }

    void Update()
    {
//        if (CubeGameManager.Instance.gameHasStarted)
//        {
//            SpawnCubes();
//        }
    }

    void SpawnCubes()
    {
        GameObject randomCube = Instantiate(cubePrefab, this.transform);
        randomCube.tag = CubeGameManager.Instance.interactableTag;
        randomCube.GetComponent<CubeSpawn>().Randomize();
//        if (Time.time > timeToSpawn && CubeGameManager.Instance.gameHasStarted)
//        {
//            timeToSpawn += spawnDelay;
//
//            GameObject randomCube = Instantiate(cubePrefab, this.transform);
//            randomCube.tag = CubeGameManager.Instance.interactableTag;
//            randomCube.GetComponent<CubeSpawn>().Randomize();
//
//        }

    }
}
