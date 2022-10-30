using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float speed;
    public bool catchable = false;
    public bool isHooked = false;


    protected void Update()
    {
        FishMovement();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WallBack")
        {
            FishManager.Instance.RemoveFish(this);
            CatchLabelManager.Instance.RemoveCatchLabel(this);
            ObjectPoolManager.Instance.Despawn(this.gameObject);
        }
        else if (other.gameObject.name == "WallCatchable")
        {
            if (GetComponent<Shark>() == null)
            {
                catchable = true;
                CatchLabelManager.Instance.AddCatchLable(this);
            }
        }
    }

    public void Init()
    {
        catchable = false;
        isHooked = false;
    }

    public void FishMovement()
    {
        if (!isHooked)
            transform.Translate(new Vector3(0, 0, -speed) * Time.deltaTime);
    }

}
