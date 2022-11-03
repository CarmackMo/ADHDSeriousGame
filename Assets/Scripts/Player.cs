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
    public float aimingFarThreshold = 10f;
    public float aimingCloseThreshold = 30f;
    public RectTransform aimingRect;
    public Transform characterTrans;
    public enum State { IDLE, AIMING, CATCHING}

    private Rigidbody rigidBody;
    private int fishCatchNum = 0;
    private int sharkHitNum = 0;
    private float fishCatchTime = 0;
    private float directionX;
    private State playerState = State.IDLE;
    private Fish targetFish;
    private Vector2 aimingVec;

    public int FishCatchNum => fishCatchNum;
    public int SharkHitNum => sharkHitNum;
    public float FishCatchTime => fishCatchTime;
    public Fish TargetFish { get { return targetFish; } set { targetFish = value; } }
    public State PlayerState => playerState;
    public Vector2 AimingVec { get { return aimingVec; } set { aimingVec = value; } }


    protected override void Awake()
    {
        base.Awake();

        rigidBody = GetComponent<Rigidbody>();    
    }

    protected override void Update()
    {
        base.Update();

        PlayerMovement();
        SelectTargetFish();
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
            if(playerState != State.CATCHING)
            {
                playerState = State.CATCHING;
                catchableFish.isHooked = true;
                GamePanel.Instance.catchingPanel.gameObject.SetActive(true);
                StartCoroutine(FishCatchCoroutine(catchableFish));
            }
        }
    }

    public void SelectTargetFish()
    {
        if (playerState == State.AIMING)
        {
            if (targetFish != null)
            {
                CatchLabel oldLabel = CatchLabelManager.Instance.GetCatchLabel(targetFish);
                oldLabel.UpdateSelectIconVisiability(false);
            }
            targetFish = null;

            foreach (Fish fish in FishManager.Instance.fishList)
            {
                Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
                Vector2 fishPos = new Vector2(fish.transform.position.x, fish.transform.position.z);
                Vector2 fishVec = fishPos - playerPos;
                float cos = Vector2.Dot(aimingVec, fishVec.normalized);
                float degAngle = Mathf.Rad2Deg * Mathf.Acos(cos);
                float distance = Vector3.Distance(transform.position, fish.transform.position);
                float aimingThreshold = distance < 35 ? aimingCloseThreshold : aimingFarThreshold;


                if (degAngle <= aimingThreshold && fish.catchable == true)
                {
                    targetFish = fish;
                    CatchLabel newLabel = CatchLabelManager.Instance.GetCatchLabel(fish);
                    newLabel.UpdateSelectIconVisiability(true);
                }

                //Debug.Log($"aimingVec: {aimingVec}, fishVec: {fishVec}, cos: {cos}, angle: {degAngle}");
                //Debug.Log("===============================================================");
            }
        }
    }

    public void TryCatchFish()
    {
        if (playerState == State.AIMING)
        {
            if (targetFish != null)
            {
                targetFish.isHooked = true;
                GamePanel.Instance.UpdateCatchingPanelVisibility(true);
                GamePanel.Instance.UpdateTouchAreaVisibility(true);
                UpdatePlayerState(State.CATCHING);
                StartCoroutine(FishCatchCoroutine(targetFish));
            }
            else
            {
                UpdatePlayerState(State.IDLE);
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
        GamePanel.Instance.UpdateCatchingPanelVisibility(true);
        CatchLabel catchLabel = CatchLabelManager.Instance.GetCatchLabel(fish);
        catchLabel.UpdateTargetIconVisiability(false);
        catchLabel.UpdateSelectIconVisiability(false);
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

        targetFish = null;
        FishManager.Instance.RemoveFish(fish);
        CatchLabelManager.Instance.RemoveCatchLabel(fish);
        ObjectPoolManager.Instance.Despawn(fish.gameObject);
        GamePanel.Instance.UpdateTargetArea(-targetStartPos, -targetAmount);
        GamePanel.Instance.UpdateCatchingPanelVisibility(false);
        GamePanel.Instance.UpdateTouchAreaVisibility(false);

        if (tmpProgressAmount >= threshold)
        {
            fishCatchNum++;
            GamePanel.Instance.UpdateFishNumText(fishCatchNum);
        }

        //yield return new WaitForSecondsRealtime(1f);
        UpdatePlayerState(State.IDLE);
        yield break;
    }

    public void UpdatePlayerState(State state = State.IDLE)
    {
        playerState = state;
    }

    public void UpdateAimingVisiability(bool visiability)
    {
        aimingRect.gameObject.SetActive(visiability);
    }

    public void UpdateAimingDirection(float degAngle = 0f)
    {

        Quaternion UIRotation = Quaternion.Euler(0, 0, degAngle);
        Quaternion CharaRotation = Quaternion.Euler(0, -degAngle, 0);
        aimingRect.localRotation = UIRotation;
        characterTrans.localRotation = CharaRotation;
    }

}
