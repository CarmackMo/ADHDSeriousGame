using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : Singleton<FishGenerator>
{
    public List<GameObject> catchableFishList = new List<GameObject>();


    public GameObject smallFish;
    public GameObject mediumFish;
    public GameObject largeFish;
    public GameObject shark;

    public Vector2 spawnInterval;
    public float leftBound;
    public float rightBound;

    private enum FishType {SMALL, MEDIUM, LARGE, SHARK};

    protected override void Start()
    {
        base.Start();

        StartCoroutine(FishSpawnCoroutine());   
    }


    protected override void Update()
    {
        base.Update();
    }


    public void AddCatchableFish(GameObject obj)
    {
        if (obj.GetComponent<Shark>() == null)
            catchableFishList.Add(obj);
    }

    public void CatchFish()
    {
        if (catchableFishList.Count > 0)
        {
            GameObject fish = catchableFishList[0];
            catchableFishList.Remove(fish);

            CatchLabelManager.Instance.RemoveCatchLabel(fish.GetComponent<Fish>());
            ObjectPoolManager.Instance.Despawn(fish);
            Player.Instance.AddFishCatchNum();
            GamePanel.Instance.UpdateFishNumText();
        }
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
                case FishType.MEDIUM:
                    ObjectPoolManager.Instance.Spawn(mediumFish, spawnPos, Quaternion.identity);
                    break;
                case FishType.LARGE:
                    ObjectPoolManager.Instance.Spawn(largeFish, spawnPos, Quaternion.identity);
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
