using UnityEngine;

public class DontDestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance { get { return instance; } }


    protected virtual void Awake()
    {
        if (instance == null)
            instance = GameObject.FindObjectOfType<T>();

        if (instance != null && instance.GetInstanceID() != this.GetInstanceID())
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void OnDestroy() { }
}
