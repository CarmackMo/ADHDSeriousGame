using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchLabelManager : Singleton<CatchLabelManager>
{
    public Camera camera;
    public Canvas canvas;
    public GameObject labelPrefab;

    private Dictionary<Fish, CatchLabel> catchLabelDict = new Dictionary<Fish, CatchLabel>();

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

            GameObject labelObj = ObjectPoolManager.Instance.Spawn(labelPrefab, labelPos, Quaternion.identity, canvas.transform);
            CatchLabel label = labelObj.GetComponent<CatchLabel>();
            label.Init();
            catchLabelDict.Add(fish, label);
        }
    }

    public void RemoveCatchLabel(Fish fish)
    {
        if (fish.GetComponent<Shark>() == null)
        {
            CatchLabel label = catchLabelDict[fish];
            catchLabelDict.Remove(fish);

            ObjectPoolManager.Instance.Despawn(label.gameObject);
        }
    }

    public CatchLabel GetCatchLabel(Fish fish)
    {
        return catchLabelDict[fish];
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
