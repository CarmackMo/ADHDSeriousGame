using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkGenerator : Singleton<SharkGenerator>
{

    public GameObject shark;

    public Vector2 spawnInterval;
    public float leftBound;
    public float rightBound;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void StartSharkSpawnCoroutine()
    {
        StartCoroutine(SharkSpawnCoroutine());
    }

    public void StopSharkSpawnCoroutine()
    {
        StopAllCoroutines();
    }

    public Shark SpawnSharkAtPos(Vector3 pos)
    {
        GameObject shark = ObjectPoolManager.Instance.Spawn(this.shark, pos, Quaternion.identity);
        shark.GetComponent<Fish>().Init();
        FishManager.Instance.AddFish(shark.GetComponent<Fish>());
        return shark.GetComponent<Shark>();
    }

    IEnumerator SharkSpawnCoroutine()
    {
        while (true)
        {
            float spawnTime = Random.Range(spawnInterval.x, spawnInterval.y);
            float spawnPosX = Random.Range(leftBound, rightBound);

            Vector3 spawnPos = transform.position;
            spawnPos.x = spawnPosX;

            GameObject shark = ObjectPoolManager.Instance.Spawn(this.shark, spawnPos, Quaternion.identity);
            shark.GetComponent<Fish>().Init();
            FishManager.Instance.AddFish(shark.GetComponent<Fish>());

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
