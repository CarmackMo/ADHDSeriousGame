using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float speed;
    public bool catchable = false;

    protected void Update()
    {
        transform.Translate(new Vector3(0, 0, -speed) * Time.deltaTime);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WallBack")
        {
            CatchLabelManager.Instance.RemoveCatchLabel(this);
            FishGenerator.Instance.catchableFishList.Remove(gameObject);
            ObjectPoolManager.Instance.Despawn(gameObject);
        }
        else if (other.gameObject.name == "WallCatchable")
        {
            FishGenerator.Instance.AddCatchableFish(gameObject);
            CatchLabelManager.Instance.AddCatchLable(this);
        }
    }
}
