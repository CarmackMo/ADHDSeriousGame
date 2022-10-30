using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Singleton<Player>
{
    public float speed;
    public int fishCatchNum = 0;


    private Rigidbody rigidBody;
    private float directionX;


    protected override void Awake()
    {
        base.Awake();

        rigidBody = GetComponent<Rigidbody>();    
    }


    protected override void Update()
    {
        base.Update();

        PlayerMovement();
    }


    public void AddFishCatchNum()
    {
        fishCatchNum++;
    }


    private void PlayerMovement()
    {
# if UNITY_ANDROID
        directionX = Input.acceleration.x;
#else
        directionX = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space))
            GamePanel.Instance.OnClickFishingBtn();
        else if (Input.GetKey(KeyCode.Escape))
        {
            GameController.Instance.PauseGame();
            MenuPanel.Instance.Show();
        }
#endif

            
        rigidBody.velocity = new Vector3(directionX * speed, 0, 0);
    }

    public void CatchFish()
    {
        Fish catchableFish = null;
        foreach(Fish fish in FishManager.Instance.fishList)
        {
            if (fish.catchable == true)
            {
                catchableFish = fish;
                break;
            }
        }

        if (catchableFish != null)
        {
            /* Instance fish catching */
            //fishCatchNum++;
            //FishManager.Instance.RemoveFish(catchableFish);
            //CatchLabelManager.Instance.RemoveCatchLabel(catchableFish);
            //ObjectPoolManager.Instance.Despawn(catchableFish.gameObject);
            //GamePanel.Instance.UpdateFishNumText();

            catchableFish.isHooked = true;
            GamePanel.Instance.catchingPanel.gameObject.SetActive(true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Shark shark = other.GetComponent<Shark>();
        if (shark != null)
        {
            DamagePanel.Instance.ShowHideEffect();
        }
    }
}
