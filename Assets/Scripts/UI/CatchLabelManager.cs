using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchLabelManager : Singleton<CatchLabelManager>
{
    public GameObject labelPrefab;
    public Dictionary<Fish, GameObject> catchLabelDict = new Dictionary<Fish, GameObject>();

    public Camera camera;
    public Canvas canvas;


    protected override void Start()
    {
        base.Start();

        camera = canvas.worldCamera;
        camera = Camera.main;
    }


    protected override void Update()
    {
        base.Update();

        UpdateCatchLabel();
    }


    public void AddCatchLable(Fish fish)
    {
        if (fish.GetComponent<Shark>() == null)
        {
            Vector3 fishPos = fish.transform.position;
            Vector3 labelPos = camera.WorldToScreenPoint(fishPos);

            GameObject label = ObjectPoolManager.Instance.Spawn(labelPrefab, labelPos, Quaternion.identity, canvas.transform);
            catchLabelDict.Add(fish, label);
        }
    }

    public void RemoveCatchLabel(Fish fish)
    {
        if (fish.GetComponent<Shark>() == null)
        {
            GameObject label = catchLabelDict[fish];
            catchLabelDict.Remove(fish);

            ObjectPoolManager.Instance.Despawn(label);
        }
    }

    private void UpdateCatchLabel()
    {
        foreach (var pair in catchLabelDict)
        {
            Vector3 fishPos = pair.Key.transform.position;
            Vector3 labelPos = camera.WorldToScreenPoint(fishPos);
            pair.Value.transform.position = labelPos;
        }
    }


}
