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

    private enum FishType {SMALL, SHARK};

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
            FishType type = (FishType)Random.Range((int)FishType.SMALL, (int)FishType.SHARK+1);

            Vector3 spawnPos = transform.position;
            spawnPos.x = spawnPosX;

            switch (type)
            {
                case FishType.SMALL:
                    ObjectPoolManager.Instance.Spawn(smallFish, spawnPos, Quaternion.identity);
                    break;
                case FishType.SHARK:
                    ObjectPoolManager.Instance.Spawn(shark, spawnPos, Quaternion.identity);
                    break;
                default:
                    ObjectPoolManager.Instance.Spawn(smallFish, spawnPos, Quaternion.identity);
                    break;
            }

            yield return new WaitForSecondsRealtime(spawnTime);
        }
    }

}
