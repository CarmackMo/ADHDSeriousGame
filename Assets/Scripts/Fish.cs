using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    protected void Update()
    {
        transform.Translate(new Vector3(0, 0, -speed) * Time.deltaTime);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WallBack")
        {
            ObjectPoolManager.Instance.Despawn(gameObject);
        }
    }
}
