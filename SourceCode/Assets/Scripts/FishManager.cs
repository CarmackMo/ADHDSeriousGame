using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : Singleton<FishManager>
{
    public List<Fish> fishList = new List<Fish>();


    public void AddFish(Fish fish)
    {
        fishList.Add(fish);
        fishList.Sort((x, y) => 
        {
            float distanceX = Vector3.Distance(x.transform.position, Player.Instance.transform.position);
            float distanceY = Vector3.Distance(y.transform.position, Player.Instance.transform.position);
            if (distanceX <= distanceY)
                return 1;
            else
                return -1;
        });
    }

    public void RemoveFish(Fish fish)
    {
        fishList.Remove(fish);
    }

    public void ClearAllFish()
    {
        while (fishList.Count > 0)
        {
            Fish fish = fishList[0];
            fishList.RemoveAt(0);
            ObjectPoolManager.Instance.Despawn(fish.gameObject);
        }
    }    
}
