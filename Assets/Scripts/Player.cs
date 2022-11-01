using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : Singleton<Player>
{
    public float speed;

    public float rhythmDeclaySpeed;
    public float rhythmGainSpeed;
    public float progressDeclaySpeed;
    public float progressGainSpeed;
    public float threshold;
    public float targetLimit;
    public float targetLowBound;
    public float targetUpBound;
    [HideInInspector]
    public float rhythmAmount = 0f;
    [HideInInspector]
    public float tmpRhythmAmount;

    public float progressAmount = 12f;
    [HideInInspector]
    public float tmpProgressAmount;
    public Image hookDirtImage;

    private Rigidbody rigidBody;
    private int fishCatchNum = 0;
    private int sharkHitNum = 0;
    private float fishCatchTime = 0;
    private float directionX;
    private bool isCatching = false;


    public bool IsCatching => isCatching;
    public int FishCatchNum => fishCatchNum;
    public int SharkHitNum => sharkHitNum;
    public float FishCatchTime => fishCatchTime;


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
    private void OnTriggerEnter(Collider other)
    {
        Shark shark = other.GetComponent<Shark>();
        if (shark != null)
        {
            sharkHitNum++;
            DamagePanel.Instance.ShowHitEffect();
        }
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

    public void HookFish()
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
            if (!isCatching)
            {
                isCatching = true;
                catchableFish.isHooked = true;
                GamePanel.Instance.catchingPanel.gameObject.SetActive(true);
                StartCoroutine(FishCatchCoroutine(catchableFish));
            }
        }
    }

    public void AddFishCatchNum()
    {
        fishCatchNum++;
    }

    public void AddRhythm()
    {
        tmpRhythmAmount = tmpRhythmAmount > threshold ? threshold : tmpRhythmAmount + rhythmGainSpeed * Time.deltaTime;
    }

    IEnumerator FishCatchCoroutine(Fish fish)
    {
        tmpRhythmAmount = rhythmAmount;
        tmpProgressAmount = progressAmount;

        float targetAmount = Random.Range(targetLowBound, targetUpBound);
        float targetStartPos = Random.Range(targetLimit, 100 - targetLimit - targetAmount);
        float targetEndPos = targetStartPos + targetAmount;

        GamePanel.Instance.UpdateCatchingRhythm(tmpRhythmAmount, threshold);
        GamePanel.Instance.UpdateTargetArea(targetStartPos, targetAmount);
        GamePanel.Instance.UpdateCatchingPanel(true);
        CatchLabel catchLabel = CatchLabelManager.Instance.GetCatchLabel(fish);
        catchLabel.UpdateCatchIconVisiability(false);
        catchLabel.UpdateProgressVisiability(true);
        catchLabel.UpdateCatchingProgress(tmpProgressAmount, threshold);

        while (tmpProgressAmount >= 0 && tmpProgressAmount < threshold)
        {
            tmpRhythmAmount = tmpRhythmAmount > 0 ? tmpRhythmAmount - (rhythmDeclaySpeed * Time.deltaTime) : 0;
            tmpProgressAmount = tmpProgressAmount > 0 ? tmpProgressAmount - progressDeclaySpeed * Time.deltaTime : 0;

            if (tmpRhythmAmount >= targetStartPos && tmpRhythmAmount <= targetEndPos)
            {
                tmpProgressAmount += progressGainSpeed * Time.deltaTime;
            }

            fishCatchTime += Time.deltaTime;
            catchLabel.UpdateCatchingProgress(tmpProgressAmount, threshold);
            GamePanel.Instance.UpdateCatchingRhythm(tmpRhythmAmount, threshold);
            yield return new WaitForEndOfFrame();
        }

        FishManager.Instance.RemoveFish(fish);
        CatchLabelManager.Instance.RemoveCatchLabel(fish);
        ObjectPoolManager.Instance.Despawn(fish.gameObject);
        GamePanel.Instance.UpdateTargetArea(-targetStartPos, -targetAmount);
        GamePanel.Instance.UpdateCatchingPanel(false);

        if (tmpProgressAmount >= threshold)
        {
            fishCatchNum++;
            GamePanel.Instance.UpdateFishNumText(fishCatchNum);
        }

        Invoke(nameof(UnsetPlayerCatching), 1f);
        yield break;
    }

    private void UnsetPlayerCatching()
    {
        isCatching = false;
    }

    public void UpdateCanvasVisiability(bool visiability)
    {
        hookDirtImage.gameObject.SetActive(visiability);
    }

}
