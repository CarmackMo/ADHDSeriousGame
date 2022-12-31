using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{

    Dictionary<GameObject, Stack<GameObject>> objectPoolDict = new Dictionary<GameObject, Stack<GameObject>>();

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion quaternion, Transform parent = null)
    {
        GameObject obj = null;

        if (!objectPoolDict.ContainsKey(prefab))
        {
            objectPoolDict.Add(prefab, new Stack<GameObject>());
        }

        Stack<GameObject> stack = objectPoolDict[prefab];

        if (stack.Count == 0)
        {
            obj = Instantiate(prefab, position, quaternion, parent);
            obj.AddComponent<ObjectPoolItem>().prefab = prefab;
        }
        else
        {
            obj = stack.Pop();
            obj.transform.position = position;
            obj.transform.rotation = quaternion;
            obj.transform.SetParent(parent);
        }

        obj.SetActive(true);
        return obj;
    }

    public void Despawn(GameObject obj)
    {
        ObjectPoolItem item = obj.GetComponent<ObjectPoolItem>();

        if (item == null || objectPoolDict.ContainsKey(item.prefab) == false)
        {
            Debug.LogFormat(" Object '{0}' wasn't spawned from a pool. Destroying it instead. ", obj.name);
            Destroy(obj);
        }
        else
        {
            obj.SetActive(false);

            if (!objectPoolDict[item.prefab].Contains(obj))
                objectPoolDict[item.prefab].Push(obj);
        }
    }
}
