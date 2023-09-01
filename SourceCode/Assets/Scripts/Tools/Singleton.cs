using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;


    protected virtual void Awake()
    {
        if (instance == null)
            instance = GameObject.FindObjectOfType<T>();

        if (instance != null && instance.GetInstanceID() != this.GetInstanceID())
            Destroy(gameObject);
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void OnDestroy() { }
}
