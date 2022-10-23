using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{

    public GameObject smallFish;
    public GameObject largeFish;
    public GameObject mediumFish;
    public GameObject shark;

    public Vector2 spawnInterval;
    public float leftBound;
    public float rightBound;


    private void Start()
    {
        StartCoroutine(FishSpawnCoroutine());   
    }


    void Update()
    {
        
    }


    IEnumerator FishSpawnCoroutine()
    {
        while (true)
        {
            float spawnTime = Random.Range(spawnInterval.x, spawnInterval.y);
            float spawnPosX = Random.Range(leftBound, rightBound);

            Vector3 spawnPos = transform.position;
            spawnPos.x = spawnPosX;

            Instantiate(smallFish, spawnPos, Quaternion.identity);

            yield return new WaitForSecondsRealtime(spawnTime);
        }
    }

}
